using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public class ServerBattleField
    {
        private Dictionary<int, ServerBubble> m_bubbleDic;
        private Dictionary<GridPosition, ServerBubble> m_gridBubbleDic;
        private ELevelMapType m_mapType;
        private int m_nextId;
        private int m_minCol;
        private float m_bubbleSpeed = 1;

        private Vector2 m_battleFieldOrign;

        private int NewBubbleId
        {
            get { return m_nextId++; }
        }

        public ServerBattleField()
        {
            m_bubbleDic = new Dictionary<int, ServerBubble>();
            m_gridBubbleDic = new Dictionary<GridPosition, ServerBubble>();
        }

        public void InitLevel(int levelId)
        {
            Clear();

            LevelMapData data = LevelConfig.Instance.GetLevelMapData(levelId);

            foreach (var ballData in data.balls)
            {
                ServerBubble bubble = new ServerBubble(ballData.id, (ELevelBallType)ballData.type, ballData.grids);
                AddBubble(bubble);
            }

            m_nextId = data.maxBallId + 1;
            m_minCol = data.minCol;

        }

        public void Clear()
        {
            m_bubbleDic.Clear();
            m_gridBubbleDic.Clear();
        }

        public void AddBubble(ServerBubble bubble)
        {
            m_bubbleDic.Add(bubble.Id, bubble);
            for (int i = 0; i < bubble.TakenGrids.Count; i++)
            {
                m_gridBubbleDic.Add(bubble.TakenGrids[i], bubble);
            }
        }

        public void RemoveBubble(ServerBubble bubble)
        {
            m_bubbleDic.Remove(bubble.Id);
            for (int i = 0; i < bubble.TakenGrids.Count; i++)
            {
                m_gridBubbleDic.Remove(bubble.TakenGrids[i]);
            }
        }

        public ServerBubble GetBubbleById(int id)
        {
            if (m_bubbleDic.ContainsKey(id))
            {
                return m_bubbleDic[id];
            }
            return null;
        }

        public ServerBubble GetBubbleByGrid(GridPosition grid)
        {
            if (m_gridBubbleDic.ContainsKey(grid))
            {
                return m_gridBubbleDic[grid];
            }
            return null;
        }

        private void ResetVisitFlag()
        {
            for (int i = 0; i < m_bubbleDic.Count; i++)
            {
                m_bubbleDic[i].isVisited = false;
            }
        }

        public void TryEraseBubble(ServerBubble bubble, ELevelBallType type, List<ServerBubble> eraseBubbles)
        {
            for (int i = 0; i < bubble.NeighborGrids.Count; i++)
            {
                ServerBubble neighbor = GetBubbleByGrid(bubble.NeighborGrids[i]);
                if (neighbor != null && neighbor.isVisited == false)
                {
                    neighbor.isVisited = true;
                    if (neighbor.CanErase(type))
                    {
                        eraseBubbles.Add(neighbor);
                        TryEraseBubble(neighbor, type, eraseBubbles);
                    }
                }
            }
        }

        private List<ServerBubble> GetRootBubbles()
        {
            List<ServerBubble> rootBubbles = new List<ServerBubble>();

            if (m_mapType == ELevelMapType.Normal)
            {
                for (int row = CellManager.MinRowValue; row <= CellManager.MaxRowValue; row++)
                {
                    GridPosition grid = new GridPosition(0, row);
                    if (m_gridBubbleDic.ContainsKey(grid))
                    {
                        rootBubbles.Add(m_gridBubbleDic[grid]);
                    }
                }
            }
            else if (m_mapType == ELevelMapType.Rotation)
            {
                GridPosition grid = new GridPosition(0, 0);
                if (m_gridBubbleDic.ContainsKey(grid))
                {
                    rootBubbles.Add(m_gridBubbleDic[grid]);
                }
            }

            return rootBubbles;
        }

        private void VisitBubble(ServerBubble bubble)
        {
            if (!bubble.isVisited)
            {
                bubble.isVisited = true;
                for (int i = 0; i < bubble.NeighborGrids.Count; i++)
                {
                    ServerBubble neighbor = GetBubbleByGrid(bubble.NeighborGrids[i]);
                    if (neighbor != null)
                    {
                        VisitBubble(neighbor);
                    }
                }
            }
        }

        public void HandleFireBall(CmdFireBall cmd)
        {
            ServerBubble bubble = new ServerBubble(NewBubbleId, (ELevelBallType)cmd.BallType, new List<GridPosition>() { cmd.DestGrid });

            // 碰撞检测Here

            ResetVisitFlag();
            List<ServerBubble> eraseBubbles = new List<ServerBubble>();
            TryEraseBubble(bubble, bubble.Type, eraseBubbles);
            if (eraseBubbles.Count > 2)
            {
                // Send CmdFireBallResponse
                CmdFireBallResponse response = new CmdFireBallResponse(cmd.Side, (int)cmd.Type, cmd.StartPosition, cmd.Dir, cmd.CollisionPoint, cmd.DestGrid, bubble.Id, m_bubbleSpeed, 0);
                BattleServer.Instance.SendResponse(response);

                List<int> eraseIds = new List<int>();
                for (int i = 0; i < eraseBubbles.Count; i++)
                {
                    eraseIds.Add(eraseBubbles[i].Id);
                    RemoveBubble(eraseBubbles[i]);
                }

                // Send CmdEraseBall
                CmdEraseBall command = new CmdEraseBall(cmd.Side, 0, eraseIds, bubble.Id);
                BattleServer.Instance.SendResponse(command);

                ResetVisitFlag();

                List<ServerBubble> rootBubbles = GetRootBubbles();
                for (int i = 0; i < rootBubbles.Count; i++)
                {
                    VisitBubble(rootBubbles[i]);
                }

                List<ServerBubble> fallenBubbles = new List<ServerBubble>();
                for (int i = 0; i < m_bubbleDic.Count; i++)
                {
                    if (m_bubbleDic[i].isVisited == false)
                    {
                        fallenBubbles.Add(m_bubbleDic[i]);
                    }
                }

                if (fallenBubbles.Count > 0)
                {
                    List<int> fallenIds = new List<int>();
                    for (int i = 0; i < fallenBubbles.Count; i++)
                    {
                        fallenIds.Add(fallenBubbles[i].Id);
                        RemoveBubble(fallenBubbles[i]);
                    }

                    // Send CmdFallBall
                    CmdFallBall command1 = new CmdFallBall(cmd.Side, 0, eraseIds, bubble.Id);
                    BattleServer.Instance.SendResponse(command1);
                }
            }
            else
            {
                AddBubble(bubble);
                CmdFireBallResponse response = new CmdFireBallResponse(cmd.Side, (int)cmd.Type, cmd.StartPosition, cmd.Dir, cmd.CollisionPoint, cmd.DestGrid, bubble.Id, m_bubbleSpeed, 1);
                BattleServer.Instance.SendResponse(response);
            }
        }
    }
}

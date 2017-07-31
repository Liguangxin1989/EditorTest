using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public class ServerBubble
    {
        private int m_id;
        private List<GridPosition> m_takenGrids;
        private Vector2 m_position;
        private ELevelBallType m_type;
        private List<GridPosition> m_neighborGrids;

        public bool isVisited = false;

        public int Id
        {
            get { return m_id; }
        }

        public List<GridPosition> TakenGrids
        {
            get { return m_takenGrids; }
        }

        public List<GridPosition> NeighborGrids
        {
            get { return m_neighborGrids; }
        }

        public ELevelBallType Type
        {
            get { return m_type; }
        }

        public ServerBubble(int id, ELevelBallType type, List<GridPosition> grids)
        {
            m_id = id;
            m_type = type;
            m_takenGrids = grids;
        }

        private void InitNeighborGrids()
        {

        }

        public bool CanErase(ELevelBallType type)
        {
            return m_type == type;
        }
    }
}

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using Common;

namespace BubbleCouple
{
    [ExecuteInEditMode]
    public class LevelMap : MonoBehaviour
    {
        public static readonly string RootPath = "Assets/StreamingAssets/Config/";

        public static string GetLevelDataPath(int levelId)
        {
            return RootPath + "Level_" + levelId + ".dat";
        }

        [NonSerialized]
        public int LevelId = 1;

        public ELevelMapType MapType = ELevelMapType.Normal;

        public int MinCol = -50;
        public int MaxCol = 0;
        public int MinRow = -6;
        public int MaxRow = 6; 

        private float GridRadius;


        private Dictionary<GridPosition, LevelBall> Balls = new Dictionary<GridPosition, LevelBall>();

        private Dictionary<ELevelBallType, GameObject> BallPrefabs;

        private int nextBallId = 1;

        private int NewBallId
        {
            get
            {
                return nextBallId++;
            }
        }

        void Awake()
        {
            BallPrefabs = new Dictionary<ELevelBallType, GameObject>();
            GameObject redball = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/LevelEditor/Prefabs/Red.prefab");
            BallPrefabs.Add(ELevelBallType.Red, redball);
            GameObject blueball = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/LevelEditor/Prefabs/Blue.prefab");
            BallPrefabs.Add(ELevelBallType.Blue, blueball);
            GameObject greenball = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/LevelEditor/Prefabs/Green.prefab");
            BallPrefabs.Add(ELevelBallType.Green, greenball);
            GameObject yellowball = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/LevelEditor/Prefabs/Yellow.prefab");
            BallPrefabs.Add(ELevelBallType.Yellow, yellowball);
            GameObject purpleball = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/LevelEditor/Prefabs/Purple.prefab");
            BallPrefabs.Add(ELevelBallType.Purple, purpleball);
            GameObject crownball = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/LevelEditor/Prefabs/Crown.prefab");
            BallPrefabs.Add(ELevelBallType.Crown, crownball);

            GridRadius = CellManager.BubbleRadius;
        }

        // Use this for initialization
        void Start()
        {
        }

        public void LoadLevelMap()
        {
            byte[] databytes = File.ReadAllBytes(GetLevelDataPath(LevelId));
            LevelMapData data = new LevelMapData();
            data.LoadFromBytes(databytes);
            MapType = (ELevelMapType)data.mapType;

            nextBallId = data.maxBallId + 1;

            foreach (var ballData in data.balls)
            {
                ELevelBallType type = (ELevelBallType)ballData.type;
                int col = ballData.grids[0].col;
                int row = ballData.grids[0].row;
                int ballId = ballData.id;

                GameObject prefab = BallPrefabs[type];
                GameObject ball = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                ball.name = string.Format("[{0},{1}][{2}]", col, row, type.ToString());
                ball.transform.SetParent(this.transform);
                ball.transform.localPosition = GridToPosition(col, row);
                ball.hideFlags = HideFlags.HideInHierarchy;
                LevelBall newball = ball.GetComponent<LevelBall>();
                newball.Id = ballId;

                List<LevelBall> connectedBalls = new List<LevelBall>();
                GetConnectedBalls(col, row, connectedBalls);
                foreach (var b in connectedBalls)
                {
                    newball.Connections.Add(b);
                    b.Connections.Add(newball);
                }

                Balls.Add(new GridPosition(col, row), newball);
            }
        }

        public void ResetBallId()
        {
            int id = 1;
            foreach (var ball in Balls)
            {
                ball.Value.Id = id++;
            }

            nextBallId = id;
        }

        public void RandomMap()
        {
            for (int col = -20; col <= 0; col++)
            {
                for (int row = -3; row <= 3; row++)
                {
                    ELevelBallType type = (ELevelBallType)UnityEngine.Random.Range(0, 6);
                    AddBall(col, row, type);
                }
            }
        }

        void OnEnable()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetBall(int col, int row, ELevelBallType type)
        {
            GridPosition gridId = new GridPosition(col, row);
            bool needSet = true;
            if (Balls.ContainsKey(gridId))
            {
                if (Balls[gridId].Type != type)
                {
                    RemoveBall(col, row);
                }
                else
                {
                    needSet = false;
                }
            }

            if (needSet)
            {
                AddBall(col, row, type);
            }
        }

        public void AddBall(int col, int row, ELevelBallType type)
        {
            GameObject prefab = BallPrefabs[type];
            GameObject ball = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            ball.name = string.Format("[{0},{1}][{2}]", col, row, type.ToString());
            ball.transform.SetParent(this.transform);
            ball.transform.localPosition = GridToPosition(col, row);
            //ball.hideFlags = HideFlags.HideInHierarchy;
            LevelBall newball = ball.GetComponent<LevelBall>();
            newball.Id = NewBallId;
            List<LevelBall> connectedBalls = new List<LevelBall>();
            GetConnectedBalls(col, row, connectedBalls);
            foreach (var b in connectedBalls)
            {
                newball.Connections.Add(b);
                b.Connections.Add(newball);
            }

            Balls.Add(new GridPosition(col, row), newball);
        }

        public void RemoveBall(int col, int row)
        {
            GridPosition gridId = new GridPosition(col, row);
            if (Balls.ContainsKey(gridId))
            {
                LevelBall deleteball = Balls[gridId];
                List<LevelBall> connectedBalls = new List<LevelBall>();
                GetConnectedBalls(col, row, connectedBalls);
                foreach (var b in connectedBalls)
                {
                    b.Connections.Remove(deleteball);
                }
                Balls.Remove(gridId);
                DestroyImmediate(deleteball.gameObject);
            }
        }

        private void GetConnectedBalls(int col, int row, List<LevelBall> connectedBalls)
        {
            GridPosition pos = new GridPosition(col - 1, row);
            if (Balls.ContainsKey(pos))
            {
                connectedBalls.Add(Balls[pos]);
            }
            pos = new GridPosition(col + 1, row);
            if (Balls.ContainsKey(pos))
            {
                connectedBalls.Add(Balls[pos]);
            }
            pos = new GridPosition(col, row - 1);
            if (Balls.ContainsKey(pos))
            {
                connectedBalls.Add(Balls[pos]);
            }
            pos = new GridPosition(col, row + 1);
            if (Balls.ContainsKey(pos))
            {
                connectedBalls.Add(Balls[pos]);
            }
            pos = new GridPosition(col - 1, row + 1);
            if (Balls.ContainsKey(pos))
            {
                connectedBalls.Add(Balls[pos]);
            }
            pos = new GridPosition(col + 1, row + 1);
            if (Balls.ContainsKey(pos))
            {
                connectedBalls.Add(Balls[pos]);
            }
        }

        public void Export()
        {
            LevelMapData data = new LevelMapData();
            data.mapType = MapType;
            data.ballRadius = GridRadius;
            data.minCol = MaxCol;
            data.maxBallId = 0;

            foreach (var ball in Balls)
            {
                BallData ballData = new BallData();
                ballData.id = ball.Value.Id;
                ballData.type = (int)ball.Value.Type;
                ballData.localX = ball.Value.gameObject.transform.localPosition.x;
                ballData.localY = ball.Value.gameObject.transform.localPosition.y;
                ballData.grids.Add(new GridPosition(ball.Key.col, ball.Key.row));
                ballData.IsRoot = ball.Key.col == 0;
                foreach (var connect in ball.Value.Connections)
                {
                    ballData.connections.Add(connect.Id);
                }

                data.balls.Add(ballData);

                if (ballData.id > data.maxBallId)
                {
                    data.maxBallId = ballData.id;
                }
                if (ball.Key.col < data.minCol)
                {
                    data.minCol = ball.Key.col;
                }
            }

            FileStream file = new FileStream(GetLevelDataPath(LevelId), FileMode.Create);
            FileBytesWriter writer = new FileBytesWriter(file);
            data.Write(writer);
            file.Flush();
            file.Close();
        }

        void OnDrawGizmos()
        {
            Color oldColor = Gizmos.color;
            if (MapType == ELevelMapType.Normal)
            {
                DrawWall();
                Gizmos.color = Color.gray;
                for (int col = MinCol; col <= MaxCol; col++)
                {
                    for (int row = MinRow; row <= MaxRow; row++)
                    {
                        DrawGrid(col, row);
                    }
                }
                Gizmos.color = Color.yellow;
                DrawGrid(0, 0);
            }
            Gizmos.color = oldColor;
        }

        void DrawGrid(int col, int row)
        {
            float r = GridRadius * 2 / Mathf.Sqrt(3);
            Vector3 center = GridToPosition(col, row);
            for (int i = 0; i < 6; i++)
            {
                float fromAngle = i * Mathf.PI / 3.0f;
                float toAngle = (i + 1) * Mathf.PI / 3.0f;
                Vector3 from = center + new Vector3(Mathf.Cos(fromAngle), Mathf.Sin(fromAngle), 0) * r;
                Vector3 to = center + new Vector3(Mathf.Cos(toAngle), Mathf.Sin(toAngle), 0) * r;
                Gizmos.DrawLine(from, to);
                //Gizmos.Draw
            }
        }

        void DrawWall()
        {
            Gizmos.color = Color.red;
            Vector3 from = new Vector3(GridRadius, GridRadius * (MaxRow * 2 + 1), 0);
            Vector3 to = new Vector3(GridRadius, GridRadius * (MinRow * 2 - 1), 0);
            Gizmos.DrawLine(from, to);
        }

        public GridPosition PositionToGrid(Vector3 position)
        {
            return CellManager.GetCellID(new Vector2(position.x, -position.y));
        }

        public Vector2 GridToPosition(int col, int row)
        {
            CellManager.CellID cellId = new CellManager.CellID();
            cellId.x = col;
            cellId.y = row;
            return CellManager.GetCellPosition(cellId);
        }

        public bool IsValidGrid(int col, int row)
        {
            return col >= MinCol && col <= MaxCol && row >= MinRow && row <= MaxRow;
        }
    }
}
#endif

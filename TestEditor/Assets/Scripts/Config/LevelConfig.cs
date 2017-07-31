using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Common;
using System.IO;

namespace BubbleCouple
{
    public enum ELevelMapType
    {
        Normal = 1,     // 普通水平推移
        Rotation,   // 转盘
    }
    public enum ELevelBallType
    {
        None = -1,
        Red,
        Blue,
        Green,
        Yellow,
        Purple,
        Crown,
    }

    public struct GridPosition
    {
        public int col;
        public int row;

        public GridPosition(int colvalue, int rowvalue)
        {
            col = colvalue;
            row = rowvalue;
        }

        public static bool operator ==(GridPosition lhs, GridPosition rhs)
        {
            return lhs.col == rhs.col && lhs.row == rhs.row; 
        }
        public static bool operator !=(GridPosition lhs, GridPosition rhs)
        {
            return lhs.col != rhs.col || lhs.row != rhs.row;
        }

        public static implicit operator GridPosition(CellManager.CellID cellId)
        {
            return new GridPosition(cellId.x, cellId.y);
        }
    }

    public class BallData
    {
        private static readonly int MaxGrid = 7;
        private static readonly int MaxConnection = 12;

        public int id;
        public int type;
        public float localX;
        public float localY;
        public List<GridPosition> grids;
        public List<int> connections;
        private int isRoot;

        public bool IsRoot
        {
            get
            {
                return isRoot == 1;
            }
            set
            {
                isRoot = value ? 1 : 0;
            }
        }

        public BallData()
        {
            grids = new List<GridPosition>();
            connections = new List<int>();
        }

        public void Read(BytesReader buffer)
        {
            id = buffer.ReadInt32();
            type = buffer.ReadInt32();
            localX = buffer.ReadFloat();
            localY = buffer.ReadFloat();
            isRoot = buffer.ReadInt32();

            grids.Clear();
            int numOfGrids = buffer.ReadInt32();
            for (int i = 0; i < MaxGrid; i++)
            {
                int col = buffer.ReadInt32();
                int row = buffer.ReadInt32();
                if (i < numOfGrids)
                {
                    grids.Add(new GridPosition(col, row));
                }
            }

            connections.Clear();
            int numOfConnection = buffer.ReadInt32();
            for (int i = 0; i < MaxConnection; i++)
            {
                int n = buffer.ReadInt32();
                if (i < numOfConnection)
                { 
                    connections.Add(n);
                }
            }
        }

        public void Write(FileBytesWriter buffer)
        {
            buffer.WriteData(id);
            buffer.WriteData(type);
            buffer.WriteData(localX);
            buffer.WriteData(localY);
            buffer.WriteData(isRoot);
            buffer.WriteData(grids.Count);
            for (int i = 0; i < MaxGrid; i++)
            {
                if (i < grids.Count)
                {
                    buffer.WriteData(grids[i].col);
                    buffer.WriteData(grids[i].row);
                }
                else
                {
                    buffer.WriteData(0);
                    buffer.WriteData(0);
                }
            }
            buffer.WriteData(connections.Count);
            for (int i = 0; i < MaxConnection; i++)
            {
                if (i < connections.Count)
                {
                    buffer.WriteData(connections[i]);
                }
                else
                {
                    buffer.WriteData(0);
                }
            }
        }
    }

    public class LevelMapData
    {
        private static readonly uint FileFlag = 0x504d4c42;
        private static readonly int FileVersion = 1;
        private static readonly uint HeaderFlag = 0x44414548;
        private static readonly uint BallFlag = 0x4c4c4142;

        public ELevelMapType mapType;
        public List<BallData> balls;

        public int minCol;
        public int maxBallId;
        public float ballRadius;

        public LevelMapData()
        {
            mapType = ELevelMapType.Normal;
            balls = new List<BallData>();
        }

        public bool LoadFromBytes(byte[] data)
        {
            balls.Clear();

            try
            {
                BytesReader buffer = new BytesReader(data);

                uint flag = buffer.ReadUInt32();
                int version = buffer.ReadInt32(); 

                if (flag != FileFlag || version != FileVersion)
                {
                    Debug.LogError("LevelMapData format error.");
                    return false;
                }

                if (!ReadHeader(buffer))
                {
                    Debug.LogError("Level file format error.");
                    return false;
                }

                if (!ReadBalls(buffer))
                {
                    Debug.LogError("Level file format error.");
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool ReadHeader(BytesReader buffer)
        {
            uint flag = buffer.ReadUInt32();
            if (flag != HeaderFlag)
            {
                return false;
            }

            mapType = (ELevelMapType)buffer.ReadInt32();
            minCol = buffer.ReadInt32();
            maxBallId = buffer.ReadInt32();
            ballRadius = buffer.ReadFloat();

            return true;
        }

        private bool ReadBalls(BytesReader buffer)
        {
            uint flag = buffer.ReadUInt32();
            if (flag != BallFlag)
            {
                return false;
            }

            int numOfBall = buffer.ReadInt32();
            if (numOfBall > 0)
            {
                for (int i = 0; i < numOfBall; i++)
                {
                    BallData ball = new BallData();
                    ball.Read(buffer);
                    balls.Add(ball);
                }
            }

            return true;
        }

        public void Write(FileBytesWriter buffer)
        {
            buffer.WriteData(FileFlag);
            buffer.WriteData(FileVersion);
            WriteHeader(buffer);
            WriteBalls(buffer);
        }

        private void WriteHeader(FileBytesWriter buffer)
        {
            buffer.WriteData(HeaderFlag);
            buffer.WriteData((int)mapType);
            buffer.WriteData(minCol);
            buffer.WriteData(maxBallId);
            buffer.WriteData(ballRadius);
        }

        private void WriteBalls(FileBytesWriter buffer)
        {
            buffer.WriteData(BallFlag);
            buffer.WriteData(balls.Count);
            for (int i = 0; i < balls.Count; i++)
            {
                balls[i].Write(buffer);
            }
        }
    }

    public class LevelConfig : Singleton<LevelConfig>
    {
        public class LevelInfo
        {
            public int levelId;
            public int levelType;
            public int targetType;
            public int targetNum;
        }

        private Dictionary<int, LevelMapData> m_levelMapData;
        private Dictionary<int, LevelInfo> m_levelInfo;

        private int m_totalLevelCount;
        public int TotalLevelCount
        {
            get { return m_totalLevelCount; }
        }

        private LevelConfig()
        {
            m_levelMapData = new Dictionary<int, LevelMapData>();
            m_levelInfo = new Dictionary<int, LevelInfo>();
        }

        public void LoadLevel(int levelId)
        {
            if (m_levelMapData.ContainsKey(levelId))
            {
                return;
            }

            LevelMapData data = new LevelMapData();
            string path = Application.streamingAssetsPath + "/Config/Level_" + levelId + ".dat";
            
            if (path.Contains("file://"))
            {
                WWW loadNetworkwww = new WWW(path);

                while (!loadNetworkwww.isDone)
                {
                }

                data.LoadFromBytes(loadNetworkwww.bytes);
                loadNetworkwww.Dispose();
            }
            else
            {
                byte[] bytes = File.ReadAllBytes(path);
                data.LoadFromBytes(bytes);
            }

            m_levelMapData.Add(levelId, data);
        }

        public bool LoadLevelInfo(string[] strContent)
        {
            m_levelInfo.Clear();
            m_totalLevelCount = 0;

            for (int index = 1; index < strContent.Length; ++index)
            {
                string[] strContents = strContent[index].Split(',');
                LevelInfo info = new LevelInfo();
                info.levelId = int.Parse(strContents[1]);
                info.levelType = int.Parse(strContents[2]);
                info.targetType = int.Parse(strContents[3]);
                info.targetNum = int.Parse(strContents[4]);
                if (info.levelId > m_totalLevelCount)
                {
                    m_totalLevelCount = info.levelId;
                }

                m_levelInfo.Add(info.levelId, info);
            }

            return true;
        }

        public LevelMapData GetLevelMapData(int levelId)
        {
            LoadLevel(levelId);

            if (m_levelMapData.ContainsKey(levelId))
            {
                return m_levelMapData[levelId];
            }

            return null;
        }

        public LevelInfo GetLevelInfo(int levelId)
        {
            if (m_levelInfo.ContainsKey(levelId))
            {
                return m_levelInfo[levelId];
            }

            return null;
        }
    }
}

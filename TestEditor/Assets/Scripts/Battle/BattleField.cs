using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BubbleCouple
{
    public class BattleField
    {
        private BattleBallInfor m_balls;
        private GameObject m_battleField;
        private BattleCmdMgr m_cmdMgr;
        private GameObject[] m_waitBalls;
        private ELevelBallType[] m_waitBallsType;
        private Vector2[] m_ballPoint;

        public Queue<ELevelBallType>[] m_ballReserve;

        private int m_myBallNum;
        private int m_mateBallNum;

        private bool [] m_isExBall;
        private float [] m_exBallTimer;
        private bool[] m_isBallUp;
        private float[] m_ballUpTimer;
        private float  m_exBallTime;
        private bool m_waitFireBall;

        private bool m_isMoveScene;
        private Vector2 m_moveSceneEndPos;
        private Vector2 m_moveSceneNormal;
        private float m_moveSceneTimer;
        private float m_moveSceneTime;

        private int m_missionNum;
        private int m_missionFinishNum;

        private Action<int, int> m_setballNumFunc;
        private Action<int, int> m_SetMissionProgressFunc;

        public bool IsExBall
        {
            get { return m_isExBall[GameCore.Instance.MySide]; }
        }

        public bool IsWaitFireBall
        {
            get { return m_waitFireBall; }
            set { m_waitFireBall = value; }
        }

        public bool IsBallUp
        {
            get { return m_isBallUp[GameCore.Instance.MySide]; }
        }
        public int MyBallNum
        {
            get { return m_myBallNum; }
        }

        public int MateBallNum
        {
            get { return m_mateBallNum; }
        }

        public Vector2[] BallPoint
        {
            get { return m_ballPoint; }
        }

        public GameObject[] WaitBalls
        {
            get { return m_waitBalls; }
        }

        public ELevelBallType[] WaitBallsType
        {
            get { return m_waitBallsType; }
        }
        public BattleBallInfor Balls
        {
            get { return m_balls; }
        }

        public GameObject BattleFieldObj
        {
            get { return m_battleField; }
        }

        public BattleField(Action<int, int> setBallNumFunc, Action<int, int> setMissionProgressFunc)
        {
            m_setballNumFunc = setBallNumFunc;
            m_SetMissionProgressFunc = setMissionProgressFunc;
        }

        public bool Init()
        {
            m_balls = new BattleBallInfor();
            m_battleField = GameObject.Find("BattleField");
            if(m_battleField == null)
            {
                Debug.LogError("Can't find Game Object BattleField~!");
                return false;
            }

            m_ballReserve = new Queue<ELevelBallType>[2];
            m_ballReserve[0] = new Queue<ELevelBallType>();
            m_ballReserve[1] = new Queue<ELevelBallType>();

            m_ballPoint = new Vector2[4];
            m_ballPoint[0] = GameObject.Find("BallPointA").transform.position;
            m_ballPoint[1] = GameObject.Find("BallPointB").transform.position;
            m_ballPoint[2] = GameObject.Find("ExBallPointA").transform.position;
            m_ballPoint[3] = GameObject.Find("ExBallPointB").transform.position;

            m_waitBalls = new GameObject[4];
            m_waitBallsType = new ELevelBallType[4];
            m_isExBall = new bool[2];
            m_exBallTimer = new float[2];
            m_isBallUp = new bool[2];
            m_ballUpTimer = new float[2];
            //for (int i = 0; i != m_waitBalls.Length; ++ i)
            //{
            //    m_waitBallsType[i] = (ELevelBallType) Random.Range(0, 4);
            //    m_waitBalls[i] = BattleResource.Instance.GetBallResource(m_waitBallsType[i]);
            //    m_waitBalls[i] = GameObject.Instantiate(m_waitBalls[i]) as GameObject;
            //    m_waitBalls[i].GetComponent<Collider2D>().enabled = false;
            //}
            //SetBallPosition(0);
            //SetBallPosition(1);

            m_cmdMgr = new BattleCmdMgr();
            m_cmdMgr.Init(this);
            m_exBallTime = ConfigHelper.GetSysConfig(ESysConfig.BulletSwitchTime) * 0.001f;

            return true;
        }

        public void Reset()
        {
            m_ballReserve[0].Clear();
            m_ballReserve[1].Clear();
            for(int i = 0; i != m_waitBalls.Length; ++ i)
            {
                if(m_waitBalls[i] != null)
                {
                    GameObject.Destroy(m_waitBalls[i]);
                    m_waitBallsType[i] = ELevelBallType.None;
                }
            }
        }

        public void CheckBallDisplay(int side)
        {
            if (m_waitBalls[side] == null)
            {
                CreateBall(side);
            }

            if (m_waitBalls[side + 2] == null && !m_isBallUp[side])
            {
                CreateBall(side + 2);
            }
            SetBallNum(side, CountBallNum(side));
        }

        public int CountBallNum(int side)
        {
            int num = m_ballReserve[side].Count;
            if (m_waitBalls[side] != null)
            {
                ++num;
            }
            if (m_waitBalls[side + 2] != null)
            {
                ++num;
            }

            return num;
        }

        public bool CreateBall(int side)
        {
            if(m_ballReserve[side % 2].Count > 0)
            {
                m_waitBallsType[side] = m_ballReserve[side % 2].Dequeue();
                m_waitBalls[side] = BattleResource.Instance.GetBallResource(m_waitBallsType[side]);
                if(m_waitBalls[side] != null)
                {
                    m_waitBalls[side] = GameObject.Instantiate(m_waitBalls[side], m_ballPoint[side], Quaternion.identity);
                    m_waitBalls[side].GetComponent<Collider2D>().enabled = false;
                    return true;
                }
                else
                {
                    Debug.LogError("Can't Load balls: " + m_waitBallsType[side]);
                }
            }

            return false;
        }

        public void ExChangeBall(int side)
        {
            GameObject ballTmp = m_waitBalls[side];
            ELevelBallType ballTypeTmp = m_waitBallsType[side];

            m_waitBalls[side] = m_waitBalls[side + 2];
            m_waitBallsType[side] = m_waitBallsType[side + 2];
            m_waitBalls[side + 2] = ballTmp;
            m_waitBallsType[side + 2] = ballTypeTmp;
            m_isExBall[side] = true;
            m_exBallTimer[side] = 0f;
        }

        public void SetBallReady(int side)
        {
            if (m_waitBalls[side] != null)
            {
                GameObject.Destroy(m_waitBalls[side]);
                m_waitBalls[side] = null;
                m_waitBallsType[side] = ELevelBallType.None;
            }
            if (m_waitBalls[side + 2] != null)
            {
                m_waitBalls[side] = m_waitBalls[side + 2];
                m_waitBallsType[side] = m_waitBallsType[side + 2];
                m_waitBalls[side + 2] = null;
                m_waitBallsType[side + 2] = ELevelBallType.None;
            }
            SetBallNum(side, CountBallNum(side));
            if (side == GameCore.Instance.MySide)
            {
                IsWaitFireBall = false;
            }
            m_ballUpTimer[side] = 0f;
            m_isBallUp[side] = true;
        }

        public void UpDataMissionProgress(int num)
        {
            m_missionFinishNum += num;
            m_SetMissionProgressFunc(m_missionFinishNum, m_missionNum);
        }

        public void SetSceneTransform(Vector2 translate)
        {
            m_isMoveScene = true;
            m_moveSceneEndPos = translate;
            Vector2 moveSceneStartPos = m_battleField.transform.position;
            m_moveSceneNormal = (translate - moveSceneStartPos).normalized;
            m_moveSceneTimer = 0f;
            m_moveSceneTime = Mathf.Abs(translate.x - moveSceneStartPos.x) /
                              (ConfigHelper.GetSysConfig(ESysConfig.SceneTranslateSpeed)*0.01f);
        }

        public void AddCommad(BattleCommand cmd)
        {
            m_cmdMgr.AddCommand(cmd);
        }

        public void InitLevel(LevelMapData data)
        {
            foreach (var ball in m_balls.Values)
            {
                GameObject.Destroy(ball);
            }

            m_balls.Clear();

            foreach(var ballData in data.balls)
            {
                GameObject res = BattleResource.Instance.GetBallResource((ELevelBallType)ballData.type);
                if(res != null)
                {
                    GameObject ball = GameObject.Instantiate(res, m_battleField.transform);
                    ball.transform.localPosition = new Vector3(ballData.localX, ballData.localY, 0);
                    m_balls.AddBall(ballData.id, ball, (ELevelBallType)ballData.type);
                }
            }

            m_missionFinishNum = 0;
            m_missionNum = LevelConfig.Instance.GetLevelInfo(GameCore.Instance.m_globalData.CurrLevel).targetNum;
            UpDataMissionProgress(0);
        }

        public void SetBallNum(int side, int ballNum)
        {
            if (side == GameCore.Instance.MySide)
            {
                m_myBallNum = ballNum;
            }
            else
            {
                m_mateBallNum = ballNum;
            }

            m_setballNumFunc(m_myBallNum, m_mateBallNum);
        }

        public void Update(float fDeltaTime)
        {
            m_cmdMgr.Update(fDeltaTime);

            if (m_isExBall[0])
            {
                RunExBall(0, fDeltaTime);
            }

            if (m_isExBall[1])
            {
                RunExBall(1, fDeltaTime);
            }

            if (m_isBallUp[0])
            {
                RunBallUp(0, fDeltaTime);
            }

            if (m_isBallUp[1])
            {
                RunBallUp(1, fDeltaTime);
            }

            if (m_isMoveScene)
            {
                m_moveSceneTimer += fDeltaTime;
                if (m_moveSceneTimer > m_moveSceneTime)
                {
                    m_battleField.transform.position = m_moveSceneEndPos;
                    m_isMoveScene = false;
                }
                else
                {
                    Vector2 pos = m_battleField.transform.position;
                    pos += m_moveSceneNormal * ConfigHelper.GetSysConfig(ESysConfig.SceneTranslateSpeed) * 0.01f * fDeltaTime;
                    m_battleField.transform.position = pos;
                }
            }
        }

        private void RunBallUp(int side, float fDeltaTime)
        {
            m_ballUpTimer[side] += fDeltaTime;
            if (m_waitBalls[side] != null)
            {
                m_waitBalls[side].transform.position = Vector2.Lerp(m_ballPoint[side + 2], m_ballPoint[side],
                    m_ballUpTimer[side] / m_exBallTime);
            }

            if (m_ballUpTimer[side] > m_exBallTime)
            {
                CreateBall(side + 2);
                m_isBallUp[side] = false;
            }
        }

        private void RunExBall(int side, float fDeltaTime)
        {
            m_exBallTimer[side] += fDeltaTime;
            if (m_waitBalls[side] != null)
            {
                m_waitBalls[side].transform.position = Vector2.Lerp(m_ballPoint[side + 2], m_ballPoint[side],
                    m_exBallTimer[side] / m_exBallTime);
            }
            if (m_waitBalls[side + 2] != null)
            {
                m_waitBalls[side + 2].transform.position = Vector2.Lerp(m_ballPoint[side], m_ballPoint[side + 2],
                    m_exBallTimer[side] / m_exBallTime);
            }
            if(m_exBallTimer[side] > m_exBallTime)
            {
                m_isExBall[side] = false;
            }
        }

        public void Destroy()
        {

        }
    }
}

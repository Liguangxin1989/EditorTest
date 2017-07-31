using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace BubbleCouple
{
    [CustomEditor(typeof(LevelMap))]
    public class LevelMapEditor : Editor
    {
        private enum EEditMode
        {
            View = 0,
            RedBall,
            BlueBall,
            GreenBall,
            YellowBall,
            PurpleBall,
            RandomBall,
            CrownBall,
            Erase,
            Max,
        }

        private string[] m_toolbarItem = new string[(int)EEditMode.Max] { "观察", "绘制红球", "绘制蓝球", "绘制绿球", "绘制黄球", "绘制紫球", "随机绘制", "绘制皇冠", "擦除" };

        private EEditMode m_currentMode = EEditMode.View;
        private int m_selectedMode = 0;

        private LevelMap m_target;
        
        void OnEnable()
        {
            m_target = (LevelMap)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("关卡ID：" + m_target.LevelId);
            base.OnInspectorGUI();
            if (GUILayout.Button("生成随机地图"))
            {
                m_target.RandomMap();
            }
            //if (GUILayout.Button("修复ID"))
            //{
            //    m_target.ResetBallId();
            //}
            if (GUILayout.Button("导出"))
            {
                m_target.Export();
            }
        }

        void OnSceneGUI()
        {
            DrawToolBar();

        }


        void DrawToolBar()
        {
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(10, 10, 480, 40));
            m_selectedMode = GUILayout.Toolbar(m_selectedMode, m_toolbarItem, GUILayout.ExpandHeight(true));
            GUILayout.EndArea();
            Handles.EndGUI();

            if ((int)m_currentMode != m_selectedMode)
            {
                m_currentMode = (EEditMode)m_selectedMode;
            }

            if (m_currentMode == EEditMode.View)
            {
                Tools.current = Tool.View;
            }
            else
            {
                Tools.current = Tool.None;
            }

            SceneView.currentDrawingSceneView.in2DMode = true;

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            Camera camera = SceneView.currentDrawingSceneView.camera;
            Vector3 mousePosition = Event.current.mousePosition;
            mousePosition = new Vector2(mousePosition.x, camera.pixelHeight - mousePosition.y);
            Vector3 worldPos = camera.ScreenToWorldPoint(mousePosition);
            GridPosition gridPos = m_target.PositionToGrid(worldPos);
            int col = gridPos.col;
            int row = gridPos.row;
            Debug.Log(string.Format("{0}, {1}", col, row));

            if (m_target.IsValidGrid(col, row) && (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag))
            {
                if (m_currentMode == EEditMode.Erase)
                {
                    Erase(col, row);
                }
                else if (m_currentMode != EEditMode.View)
                {
                    Paint(col, row);
                }
            }
        }

        private void Paint(int col, int row)
        {
            m_target.SetBall(col, row, CurrentPaintType);
        }

        private ELevelBallType CurrentPaintType
        {
            get
            {
                ELevelBallType type = ELevelBallType.Red;
                switch (m_currentMode)
                {
                    case EEditMode.RedBall:
                        type = ELevelBallType.Red;
                        break;
                    case EEditMode.BlueBall:
                        type = ELevelBallType.Blue;
                        break;
                    case EEditMode.GreenBall:
                        type = ELevelBallType.Green;
                        break;
                    case EEditMode.YellowBall:
                        type = ELevelBallType.Yellow;
                        break;
                    case EEditMode.PurpleBall:
                        type = ELevelBallType.Purple;
                        break;
                    case EEditMode.RandomBall:
                        type = (ELevelBallType)Random.Range((int)ELevelBallType.Red, (int)ELevelBallType.Crown);
                        break;
                    case EEditMode.CrownBall:
                        type = ELevelBallType.Crown;
                        break;
                }

                return type;
            }
        }

        private void Erase(int col, int row)
        {
            m_target.RemoveBall(col, row);
        }
    }
}

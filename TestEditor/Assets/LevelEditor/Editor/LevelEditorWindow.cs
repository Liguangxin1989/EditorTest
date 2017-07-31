using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace BubbleCouple
{
    public class LevelEditorWindow : EditorWindow
    {
        private int m_maxLevelId = 0;
        private int m_selected = 0;
        private string[] m_levelString;

        void OnEnable()
        {
            int levelIndex = 1;
            while (File.Exists(LevelMap.GetLevelDataPath(levelIndex)))
            {
                levelIndex++;
            }
            m_maxLevelId = levelIndex - 1;
            if (m_maxLevelId > 0)
            {
                m_levelString = new string[m_maxLevelId];
                for (int i = 0; i < m_maxLevelId; i++)
                {
                    m_levelString[i] = (i + 1).ToString();
                }
            }
        }

        void OnGUI()
        {
            if (m_maxLevelId > 0)
            {
                m_selected = EditorGUILayout.Popup("选择要编辑的关卡", m_selected, m_levelString);
                if (GUILayout.Button("编辑关卡"))
                {
                    CreateLevelEditorScene(m_selected + 1, true);
                }
            }

            if (GUILayout.Button("新建关卡"))
            {
                CreateLevelEditorScene(m_maxLevelId + 1);
            }
        }

        private void CreateLevelEditorScene(int levelId, bool bLoad = false)
        {
            NewScene();
            GameObject levelMapGo = new GameObject("LevelMap");
            levelMapGo.transform.position = Vector3.zero;
            LevelMap levelMap = levelMapGo.AddComponent<LevelMap>();
            levelMapGo.transform.hideFlags = HideFlags.NotEditable;
            levelMap.LevelId = levelId;
            if (bLoad)
            {
                levelMap.LoadLevelMap();
            }
            Close();
        }

        private void NewScene()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        }

        private void ClearScene()
        {
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects)
            {
                GameObject.DestroyImmediate(go);
            }
        }
    }
}

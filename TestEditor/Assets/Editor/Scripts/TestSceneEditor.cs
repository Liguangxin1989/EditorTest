using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TestScene))]
public class TestSceneEditor : Editor {

    TestScene m_target;

    void OnEnable()
    {
        m_target = (TestScene)target;
    }

    void OnSceneGUI()
    {
        Tools.current = Tool.None;
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(0, 0, 200, 200));
        GUILayout.Label("Hello");
        if (GUILayout.Button("Change Color"))
        {
            m_target.ChangeColor();
        }
        GUILayout.EndArea();
        Handles.EndGUI();

        Vector3 newPosition = Handles.FreeMoveHandle(m_target.transform.position, m_target.transform.rotation, 1, Vector3.one, Handles.RectangleCap);
        m_target.transform.position = newPosition;

        if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp)
        {
            m_target.ToggleShape();
            SceneView.lastActiveSceneView.Repaint();
        }
    }
}

#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestInspector))]
public class TestClassEditor : Editor
{
    TestInspector m_target;
    bool m_defaultMode = true;

    void OnEnable()
    {
        m_target = (TestInspector)target;
    }

    public override void OnInspectorGUI()
    {
        m_defaultMode = EditorGUILayout.Toggle("显示默认样式", m_defaultMode);
        if (m_defaultMode)
        {
            base.OnInspectorGUI();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Name");
            m_target.Name = EditorGUILayout.TextField(m_target.Name);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Age");
            m_target.Age = EditorGUILayout.IntField(m_target.Age);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }

}
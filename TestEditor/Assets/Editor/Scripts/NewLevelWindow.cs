using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NewLevelWindow : EditorWindow
{
    [MenuItem("Window/New Level")]
    public static void NewLevel()
    {
        EditorWindow.GetWindow<NewLevelWindow>();
    }

    private int width = 7;
    private int height = 7;

    void OnEnable()
    {

    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("新建关卡");
        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("创建关卡"))
        {
            // do something to create new level
            this.Close();
        }
    }
}

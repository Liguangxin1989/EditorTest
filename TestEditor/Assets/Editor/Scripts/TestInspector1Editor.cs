using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestInspector1))]
public class TestInspector1Editor : Editor
{
    private int selectedPageIndex = 0;
    private int selectedToolIndex = -1;
    Texture2D res;
    private int[] toolCount = new int[3] { 12, 3, 21 };

	void OnEnable () {
        res = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Texture/img_bu_cj_bubble_blue.png");
	}

    public override void OnInspectorGUI()
    {
        int index = GUILayout.Toolbar(selectedPageIndex, new string[3] { "Tools1", "Tools2", "Tools3" });

        if (selectedPageIndex != index)
        {
            selectedPageIndex = index;
            selectedToolIndex = -1;
        }

        EditorGUILayout.BeginVertical("box");

        int count = toolCount[selectedPageIndex];
        int id = 0;
        while (id < count)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = id; i < id + 5 && i < count; i++)
            {
                GUIContent content = new GUIContent();
                content.text = "Tool" + selectedPageIndex + "_" + i;
                content.image = res;
                GUIStyle style = new GUIStyle(GUI.skin.button);
                style.alignment = TextAnchor.LowerCenter;
                style.imagePosition = ImagePosition.ImageAbove;
                style.fixedWidth = 60;
                style.fixedHeight = 60;
                if (i == selectedToolIndex)
                {
                    style.normal.background = style.active.background;
                    style.normal.textColor = Color.red;
                }
                if (GUILayout.Button(content, style))
                {
                    selectedToolIndex = i;
                }
            }
            id += 5;
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace BubbleCouple
{
    public class LevelEditorMenu
    {
        [MenuItem("LevelEditor/LevelEditor")]
        public static void ShowLevelEditorWindow()
        {
            EditorWindow.GetWindow(typeof(LevelEditorWindow));
        }


    }
}

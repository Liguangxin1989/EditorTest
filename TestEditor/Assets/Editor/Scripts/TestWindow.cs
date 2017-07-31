using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestWindow : EditorWindow
{
    List<Rect> m_windows = new List<Rect>();

    [MenuItem("Tool/tool1")]
    static void ShwoTestWindow ()
    {
        TestWindow window = EditorWindow.GetWindow<TestWindow>(false, "TestWindow", true);
        window.Init();
    }


    private void Init()
    {
        for (int i = 0; i < 3; i++)
        {
            m_windows.Add(new Rect(10*i, 10*i, 200, 200));
        }
    }

    private void OnGUI()
    {
        BeginWindows();
       
        for (int i = 0; i < m_windows.Count; i++)
        {
            if (2 * i + 1 < m_windows.Count)
                DrawLineAndArrow(m_windows[i], m_windows[2 * i + 1]);
            if (2 * (i + 1) < m_windows.Count)
                DrawLineAndArrow(m_windows[i], m_windows[2 * (i + 1)]);
        }

        DrawaWindows();
        EndWindows();

    }

    void DrawaWindows()
    {
        if(m_windows.Count >0)
        {
            for (int i = 0; i < m_windows.Count; i++)
            {
                m_windows[i] = GUI.Window(i, m_windows[i], createWindows, "窗口" + i);
            }
        }
    }

    void createWindows(int i)
    {
        GUILayout.Label("当前是 " + i);
        GUI.DragWindow();
    }

        
    void DrawLineAndArrow (Rect head , Rect tail)
    {
        Vector3 startPos = new Vector3(head.x + head.width , head.y + head.height/2  , 0);
        Vector3 endPos = new Vector3(tail.x, tail.y + tail.height/2 , 0);
        Vector3 startTangent = startPos +Vector3.right * 50;
        Vector3 endTangent = endPos +Vector3.left * 50;

        Handles.DrawBezier(startPos, endPos, startTangent, endTangent, Color.green, null, 1);

        DrawArrow(startPos,endPos);
    }

    int detal = 10;
    void DrawArrow(Vector3 startPos, Vector3 endPos )
    {
        //Quaternion rotate = Quaternion.LookRotation(endPos - startPos, Vector3.up);
        //float _s = HandleUtility.GetHandleSize(endPos);
        //Handles.DrawLine( endPos+ rotate * new Vector3(0, 1, 1)  * _s , endPos);
        Handles.color = Color.green;
        Handles.DrawLine(endPos + new Vector3(-10, -10, 0), endPos);
        Handles.DrawLine(endPos + new Vector3(-10, 10, 0), endPos);

    }

}

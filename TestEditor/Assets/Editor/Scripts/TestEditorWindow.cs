using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestEditorWindow : EditorWindow
{
    [MenuItem("Window/TestEditor")]
    public static void ShowTestEditor()
    {
        EditorWindow.GetWindow<TestEditorWindow>();
    }

    enum EDragType
    {
        None,
        Divider,
        SrcNode,
        DestNode,
    }

    private EDragType dragType = EDragType.None;
    private float leftPanelWidth = 200;
    private Vector2 lastMousePos;
    Texture2D bgTexture;

    private Vector2 posSrc = new Vector2(100, 100);
    private Vector2 posDest = new Vector2(400, 300);
    private Vector2 nodeSize = new Vector2(100, 30);

    private bool isSelectLine = false;

    void OnEnable()
    {
        bgTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Texture/bg.png");
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        Rect rectLeftPanel = EditorGUILayout.BeginVertical(new GUILayoutOption[]
        {
            GUILayout.Width(leftPanelWidth)
        });
        EditorGUILayout.LabelField("Hello");
        EditorGUILayout.EndVertical();
        leftPanelWidth = rectLeftPanel.width;

        GUIStyle vDivider = new GUIStyle(GUI.skin.box);
        vDivider.name = "vDivider";
        vDivider.margin = new RectOffset(0, 0, 0, 0);
        vDivider.padding = new RectOffset(0, 0, 0, 0);
        vDivider.border = new RectOffset(0, 0, 0, 0);
        vDivider.normal.background = null;
        GUILayout.Box(string.Empty, vDivider, new GUILayoutOption[]
        {
            GUILayout.Width(2),
            GUILayout.ExpandHeight(true)
        });
        Rect vDivRect = GUILayoutUtility.GetLastRect();
        EditorGUIUtility.AddCursorRect(vDivRect, MouseCursor.ResizeHorizontal);

        Vector2 scrollViewPosition = new Vector2(vDivRect.x + vDivRect.width, vDivRect.y);

        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.normal.background = bgTexture;
        EditorGUILayout.BeginScrollView(Vector2.zero, false, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, gUIStyle, new GUILayoutOption[]
        {
            GUILayout.ExpandWidth(true)
        });

        Color oldColor = Handles.color;
        Handles.color = isSelectLine ? Color.red : Color.yellow;
        Handles.DrawLine(posSrc, posDest);
        Handles.color = oldColor;

        Rect rectSrc = new Rect(posSrc - nodeSize * 0.5f, nodeSize);
        Rect rectDest = new Rect(posDest - nodeSize * 0.5f, nodeSize);
        GUI.Box(rectSrc, "Src");
        GUI.Box(rectDest, "Dest");
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();

        bool needRepaint = false;

        Vector2 mousePos = Event.current.mousePosition;
        if (Event.current.type == EventType.MouseDown)
        {
            Debug.Log("rectSrc" + rectSrc.ToString());
            Debug.Log("scrollViewPosition: " + scrollViewPosition.ToString());
            Debug.Log("mousePos - scrollViewPosition: " + (mousePos - scrollViewPosition).ToString());
            if(vDivRect.Contains(mousePos))
            {
                dragType = EDragType.Divider;
            }
            else if (rectSrc.Contains(mousePos - scrollViewPosition))
            {
                dragType = EDragType.SrcNode;
            }
            else if (rectDest.Contains(mousePos - scrollViewPosition))
            {
                dragType = EDragType.DestNode;
            }
            if (dragType != EDragType.None)
            {
                lastMousePos = mousePos;
            }
            else
            {
                isSelectLine = IsHitLine(mousePos - scrollViewPosition, posSrc, posDest);
                needRepaint = true;
            }
        }
        else if (Event.current.type == EventType.MouseDrag)
        {
            if (dragType != EDragType.None)
            {
                Vector2 delta = mousePos - lastMousePos;
                if (dragType == EDragType.Divider)
                {
                    leftPanelWidth += delta.x;
                }
                else if (dragType == EDragType.SrcNode)
                {
                    posSrc += delta;
                }
                else if (dragType == EDragType.DestNode)
                {
                    posDest += delta;
                }

                lastMousePos = mousePos;
                needRepaint = true;
            }
        }
        else if (Event.current.type == EventType.MouseUp)
        {
            dragType = EDragType.None;
        }

        if (needRepaint)
        {
            needRepaint = false;
            base.Repaint();
        }
    }

    bool IsHitLine(Vector2 point, Vector2 p1, Vector2 p2)
    {
        float disp1point = Vector2.Distance(p1, point);
        float disp2point = Vector2.Distance(p2, point);
        float disp1p2 = Vector2.Distance(p1, p2);

        if (disp1point < disp1p2 && disp2point < disp1p2)
        {
            float dis = HandleUtility.DistancePointToLine(point, p1, p2);
            return dis < 5;
        }

        return false;
    }
}

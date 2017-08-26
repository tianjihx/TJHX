using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(BattleMapManager))]
class BattleGridEditor : Editor 
{
    private bool[,] map;
    Point MouseHitPoint;
    Vector3 MouseHitPosition;
    private bool editMode = false;

    private bool showGridBtnPressed = false;
    private bool createBtnPressed = false;
    private bool saveBtnPressed = false;
    private bool loadBtnPressed = false;

    private int width = 100;
    private int height = 100;

    Vector3 lookAtPosition;
    Vector2 lastMousePosition;

    private void OnSceneGUI()
    {
        BattleMapManager battleMap = target as BattleMapManager;

        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(20, 20, 100, 400));
        if (GUILayout.Button(showGridBtnPressed ? "隐藏战斗网格" : "显示战斗网格"))
        {
            showGridBtnPressed = !showGridBtnPressed;
            if (showGridBtnPressed)
            {
                Tools.lockedLayers = ~0;
                SceneView.currentDrawingSceneView.orthographic = true;
                lookAtPosition = new Vector3(width / 2, 0, height / 2);
                SceneView.currentDrawingSceneView.LookAt(lookAtPosition, Quaternion.Euler(90, 0, 0));
            }
            else
            {
                Tools.lockedLayers = 0;
                SceneView.currentDrawingSceneView.orthographic = false;
            }
        }
        if (showGridBtnPressed)
        {
            //editMode = GUILayout.Toggle(editMode, "修改战斗地图");
            GUILayout.Label("修改战斗地图");
            GUILayout.BeginHorizontal();
            int.TryParse(GUILayout.TextField(width.ToString()), out width);
            int.TryParse(GUILayout.TextField(height.ToString()), out height);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("创建新地图"))
            {
                map = new bool[width, height];
            }
            if (GUILayout.Button("保存地图"))
            {
                DB.SaveBattleMapGrid("TestScene", map);
            }
            if (GUILayout.Button("加载地图"))
            {
                map = DB.LoadBattleMapGrid("TestScene");
            }
            if (GUILayout.Button("清空地图"))
            {
                map = null;
            }


        }
        GUILayout.EndArea();
        Handles.EndGUI();


        Event e = Event.current;
        int controlId = GUIUtility.GetControlID(FocusType.Passive);
        e.GetTypeForControl(controlId);
        switch (e.GetTypeForControl(controlId))
        {
            case EventType.MouseDown:
                if (showGridBtnPressed)
                {
                    GUIUtility.hotControl = controlId;
                    if (e.button == 2)
                    {
                        lastMousePosition = GUIUtility.GUIToScreenPoint(e.mousePosition);
                    }
                    if (e.button == 0)
                    {
                        if (map != null)
                        {
                            //Debug.Log("change " + MouseHitPoint);
                            map[MouseHitPoint.x, MouseHitPoint.y] = true;
                        }
                    }
                    if (e.button == 1)
                    {
                        if (map != null)
                        {
                            //Debug.Log("change " + MouseHitPoint);
                            map[MouseHitPoint.x, MouseHitPoint.y] = false;
                        }
                    }
                }
                break;
            case EventType.MouseMove:
                if (showGridBtnPressed)
                {
                    GUIUtility.hotControl = controlId;
                    UpdateMouseHit(e);
                }
                break;
            case EventType.MouseDrag:
                if (!showGridBtnPressed)
                    break;
                UpdateMouseHit(e);
                if (e.button == 0)
                {
                    if (map != null)
                    {
                        //Debug.Log("change " + MouseHitPoint);
                        map[MouseHitPoint.x, MouseHitPoint.y] = true;
                    }
                }
                if (e.button == 1)
                {
                    if (map != null)
                    {
                        //Debug.Log("change " + MouseHitPoint);
                        map[MouseHitPoint.x, MouseHitPoint.y] = false;
                    }
                }
                if (e.button == 2)
                {
                    Vector2 delta = GUIUtility.GUIToScreenPoint(e.mousePosition) - lastMousePosition;
                    lookAtPosition += Vector3.Scale(new Vector3(delta.x, 0, delta.y), new Vector3(-0.1f, 0, 0.1f) * SceneView.currentDrawingSceneView.camera.orthographicSize / 35);
                    lastMousePosition = GUIUtility.GUIToScreenPoint(e.mousePosition);
                    SceneView.currentDrawingSceneView.LookAtDirect(lookAtPosition, Quaternion.Euler(90,0,0));
                }
                break;
        }


        if (showGridBtnPressed)
        {
            DrawGrid(MouseHitPoint, Color.green);
            Handles.Label(MouseHitPoint.ToVector3() + new Vector3(0, 0, 1f), MouseHitPoint.ToString() + ", " + MouseHitPosition);
            if (map != null)
            {
                int width = map.GetLength(0);
                int height = map.GetLength(1);
                
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        if (map[x, y])
                            DrawGrid(new Point(x, y), Color.red);
                    }
                }
                DrawMap(width, height, Color.blue);
            }
            SceneView.RepaintAll();
            
        }


        
    }

    private void UpdateMouseHit(Event e)
    {
        Camera viewCam = SceneView.currentDrawingSceneView.camera;
        RaycastHit hit;
        Vector2 posInSceneView = GUIUtility.GUIToScreenPoint(e.mousePosition) -
            new Vector2(SceneView.lastActiveSceneView.position.x, SceneView.lastActiveSceneView.position.y);
        posInSceneView.y = SceneView.lastActiveSceneView.position.height - posInSceneView.y;
        posInSceneView.x = Mathf.Clamp(posInSceneView.x, 0, SceneView.currentDrawingSceneView.position.width - 1);
        posInSceneView.y = Mathf.Clamp(posInSceneView.y, 0, SceneView.currentDrawingSceneView.position.height - 1);
        //Debug.Log(posInSceneView);
        if (!viewCam.pixelRect.Contains(posInSceneView))
            return;
        if (Physics.Raycast(viewCam.ScreenPointToRay(posInSceneView), out hit, 1000, 1 << LayerMask.NameToLayer("Ground")))
        {
            MouseHitPoint = new Point(hit.point + new Vector3(0.5f, 0, 1));
            MouseHitPosition = hit.point;
        }
        
    }

    private void DrawGrid(Point pos, Color color)
    {
        Vector3 leftDown = pos.ToVector3() + new Vector3(-0.5f, 0, -0.5f);
        Vector3 rightUp = pos.ToVector3() + new Vector3(0.5f, 0, 0.5f);
        Vector3 leftUp = pos.ToVector3() + new Vector3(-0.5f, 0, 0.5f);
        Vector3 rightDown = pos.ToVector3() + new Vector3(0.5f, 0, -0.5f);
        Vector3[] verts = new Vector3[4];
        verts[0] = leftDown;
        verts[1] = leftUp;
        verts[2] = rightUp;
        verts[3] = rightDown;
        Color solidColor = color;
        solidColor.a = 0.2f;
        Handles.DrawSolidRectangleWithOutline(verts, solidColor, color);
    }

    private void DrawMap(int width, int height, Color color)
    {
        float offset = 0.5f;
        Vector3 leftDown = Point.Zero.ToVector3() + new Vector3(-offset, 0, -offset);
        Vector3 rightUp = new Point(width, height).ToVector3() + new Vector3(offset, 0, offset);
        Vector3 leftUp = new Point(0, height).ToVector3() + new Vector3(-offset, 0, offset);
        Vector3 rightDown = new Point(width, 0).ToVector3() + new Vector3(offset, 0, -offset);
        Vector3[] verts = new Vector3[4];
        verts[0] = leftDown;
        verts[1] = leftUp;
        verts[2] = rightUp;
        verts[3] = rightDown;
        Color solidColor = color;
        solidColor.a = 0.2f;
        Handles.DrawSolidRectangleWithOutline(verts, solidColor, color);
    }
}

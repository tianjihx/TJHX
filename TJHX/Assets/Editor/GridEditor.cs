using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(BattleMapManager))]
class BattleGridEditor : Editor 
{
    private bool[,] map;
    Point lastHitPoint;
    private bool editMode = false;

    private bool showGridBtnPressed = false;
    private bool createBtnPressed = false;
    private bool saveBtnPressed = false;
    private bool loadBtnPressed = false;

    private int width;
    private int height;

    Vector3 savedCamPosition;
    Quaternion savedCamRotation;
    Matrix4x4 savedCamProjection;

    private void OnSceneGUI()
    {
        Event e = Event.current;
        int controlId = GUIUtility.GetControlID(FocusType.Passive);
        e.GetTypeForControl(controlId);
        switch (e.GetTypeForControl(controlId))
        {
            case EventType.mouseDown:
                if (showGridBtnPressed)
                {
                    GUIUtility.hotControl = controlId;
                    if (map != null)
                    {
                        map[lastHitPoint.x, lastHitPoint.y] = !map[lastHitPoint.x, lastHitPoint.y];
                    }
                }
                break;
            case EventType.MouseMove:
                if (showGridBtnPressed)
                {
                    GUIUtility.hotControl = controlId;
                    Camera viewCam = SceneView.currentDrawingSceneView.camera;
                    RaycastHit hit;
                    Vector2 posInSceneView = e.mousePosition - new Vector2(0, viewCam.pixelHeight);
                    posInSceneView.y = -posInSceneView.y;
                    if (Physics.Raycast(viewCam.ScreenPointToRay(posInSceneView), out hit, 1000, 1 << LayerMask.NameToLayer("Ground")))
                    {
                        lastHitPoint = new Point(hit.point);

                    }
                }
                break;
        }


        BattleMapManager battleMap = target as BattleMapManager;

        

        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(20, 20, 100, 400));
        if (GUILayout.Button(showGridBtnPressed ? "隐藏战斗网格" : "显示战斗网格"))
        {
            showGridBtnPressed = !showGridBtnPressed;
            if (showGridBtnPressed)
            {
                Tools.lockedLayers = ~0;
                savedCamPosition = SceneView.currentDrawingSceneView.camera.transform.position;
                savedCamRotation = SceneView.currentDrawingSceneView.camera.transform.rotation;
                //savedCamProjection = SceneView.currentDrawingSceneView.camera.projectionMatrix;
                SceneView.currentDrawingSceneView.orthographic = true;
                //SceneView.currentDrawingSceneView.camera.projectionMatrix = Camera.main.projectionMatrix;
                SceneView.currentDrawingSceneView.camera.transform.position = Camera.main.transform.position;
                SceneView.currentDrawingSceneView.camera.transform.rotation = Camera.main.transform.rotation;
            }
            else
            {
                Tools.lockedLayers = 0;
                //SceneView.currentDrawingSceneView.camera.projectionMatrix = savedCamProjection;
                SceneView.currentDrawingSceneView.orthographic = false;
                SceneView.currentDrawingSceneView.camera.transform.position = savedCamPosition;
                SceneView.currentDrawingSceneView.camera.transform.rotation = savedCamRotation;
            }
        }
        if (showGridBtnPressed)
        {
            //editMode = GUILayout.Toggle(editMode, "修改战斗地图");
            GUILayout.Label("修改战斗地图");
            editMode = true;
            if (editMode)
            {
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

                }
                if (GUILayout.Button("加载地图"))
                {

                }
                if (GUILayout.Button("清空地图"))
                {
                    map = null;
                }
            }

            
        }
        GUILayout.EndArea();

        
        Handles.EndGUI();

        if (editMode)
        {
            DrawGrid(lastHitPoint, Color.green);
            if (map != null)
            {
                int width = map.GetLength(0);
                int height = map.GetLength(1);
                for (int i = 0; i < width; ++i)
                {
                    for (int j = 0; j < height; ++j)
                    {
                        if (!map[i, j])
                            DrawGrid(new Point(i, j), Color.red);
                    }
                }
            }
            SceneView.RepaintAll();
            
        }


        
    }

    private void DrawGrid(Point pos, Color color)
    {
        Vector3 leftDown = pos.ToVector3() + new Vector3(-0.5f, 0, -1);
        Vector3 rightUp = pos.ToVector3() + new Vector3(0.5f, 0, 1);
        Vector3 leftUp = pos.ToVector3() + new Vector3(-0.5f, 0, 1);
        Vector3 rightDown = pos.ToVector3() + new Vector3(0.5f, 0, -1);
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

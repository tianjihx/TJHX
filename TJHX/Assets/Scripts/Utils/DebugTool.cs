using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
class DebugTool : MonoBehaviour
{
    private static DebugTool _instance;
    public static DebugTool I
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("_DebugTool").GetComponent<DebugTool>();
            return _instance;
        }
    }

    public float width = 0;
    public float height = 0;
    public Vector2 offset = Vector2.zero;
    public bool IsShowGrid = false;

    private void Awake()
    {
        _instance = GetComponent<DebugTool>();
    }

    [MenuItem("工具/显示地形网格")]
    public static void ShowGrid()
    {
        GameObject ground = GameObject.FindGameObjectWithTag("Ground");
        Terrain terrain = ground.GetComponent<Terrain>();
        I.width = terrain.terrainData.size.x;
        I.height = terrain.terrainData.size.z;
        Debug.Log(terrain.terrainData.size);
        I.IsShowGrid = true;
    }

    public static void HideGrid()
    {
        I.IsShowGrid = false;
    }

    void OnDrawGizmos()
    {
        if (IsShowGrid)
        {
            Point start, end;
            Gizmos.color = new Color(0, 0, 1.0f, 0.5f);
            for (int i = 0 ; i < width; i++)
            {
                start.x = i;
                start.y = 0;
                end.x = i;
                end.y = (int)height;
                Gizmos.DrawLine(start.ToVector3() + new Vector3(offset.x, 0, offset.y), end.ToVector3() + new Vector3(offset.x, 0, offset.y));
            }
            for (int j = 0 ; j < height; j++)
            {
                start.x = 0;
                start.y = j;
                end.x = (int)width;
                end.y = j;
                Gizmos.DrawLine(start.ToVector3() + new Vector3(offset.x, 0, offset.y), end.ToVector3() + new Vector3(offset.x, 0, offset.y));
            }
        }

    }

    
}

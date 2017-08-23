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
            Gizmos.color = new Color(0, 0, 1.0f, 0.5f);
            for (int i = 0; i < width; i++)
            {
                Gizmos.DrawLine(new Vector3(i + offset.x, 0, 0), new Vector3(i + offset.x, 0, height));
            }
            for (int j = 0; j < height; j++)
            {
                Gizmos.DrawLine(new Vector3(0, 0, j * 2 + offset.y), new Vector3(width, 0, j * 2 + offset.y));
            }
        }

    }

    
}

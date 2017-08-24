using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Data.Sqlite;
using UnityEngine;
using Dapper;

public class DB
{
    private static DB _instance;
    private static DB Self
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DB();
                _instance.Init();
            }
            return _instance;
        }
    }

    
    private SqliteConnection conn = new SqliteConnection();
    private string connString = $"URI=file:{Application.dataPath}/DataBase/tjhx.db";
    
    private void Init()
    {
        try
        {
            conn.ConnectionString = connString;
            conn.Open();
        }
        catch (Exception e)
        {
            Debug.LogError("连接数据库失败！" + e.Message);
        }
        
    }

    public static TestData GetTestData()
    {
        string sql = "select * from TestTable";
        var a = Self.conn.Query(sql).ToArray();
        if (a.Length == 0)
            return null;
        var testData = a[0];
        return new TestData() { a = testData.a };
    }

    public static bool[,] LoadBattleMapGrid(string sceneName)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        string sql = $"select * from BattleMap where SceneName='{sceneName}'";
        var result = Self.conn.Query(sql).ToArray();
        if (result.Length == 0)
            return null;
        var mapData = result[0];
        int width = mapData.Width;
        int height = mapData.Height;
        string gridHexString = mapData.GridString;
        bool[,] map = new bool[width, height];
        for (int i = 0; i < gridHexString.Length; ++i)
        {
            char hexChar = gridHexString[i];
            ushort num = Convert.ToUInt16(hexChar.ToString(), 16);
            
            int bitIndex = 0;
            while (bitIndex < 4)
            {
                if ((num & 1) == 1)
                {
                    int y = (i * 4 + bitIndex) / width;
                    int x = (i * 4 + bitIndex) - y * width;
                    map[x, y] = true;
                }
                num = (ushort)(num >> 1);
                ++bitIndex;
            }
        }
        sw.Stop();
        Debug.Log($"加载{sceneName}场景战斗网格地图完成！耗时：{sw.ElapsedMilliseconds}ms");
        return map;
    }

    public static void SaveBattleMapGrid(string sceneName, bool[,] map)
    {
        int bitCount = 0;
        StringBuilder hexUnitSb = new StringBuilder();
        StringBuilder hexStringSb = new StringBuilder();
        for (int y = 0; y < map.GetLength(0); ++y)
        {
            for (int x = 0; x < map.GetLength(1); ++x)
            {
                hexUnitSb.Append(map[x, y] ? '1' : '0');
                ++bitCount;
                if (bitCount == 4)
                {
                    ushort hexNum = Convert.ToUInt16(hexUnitSb.ToString(), 2);
                    hexStringSb.Append(Convert.ToString(hexNum, 16));
                    hexUnitSb.Clear();
                    bitCount = 0;
                }
            }
        }
        Debug.Log(hexStringSb.ToString());
        string sql = $"select * from BattleMap where SceneName='{sceneName}'";
        if (Self.conn.Query(sql).ToArray().Length > 0)
        {
            sql = $"update BattleMap set GridString = '{hexStringSb.ToString()}' where SceneName='{sceneName}'";
            int rowUpdate = Self.conn.Execute(sql);
            if (rowUpdate != 1)
            {
                Debug.LogError("更新BattleMap发生错误！SQL:" + sql);
            }
            else
            {
                Debug.Log($"更新{sceneName}的战斗网格地图成功！");
            }
        }
        else
        {
            Debug.LogError("不存在SceneName=" + sceneName + "的表");
        }
    }
    
    

}

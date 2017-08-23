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
            short num = Convert.ToInt16(hexChar+"", 16);
            int bitIndex = 0;
            while (bitIndex < 16)
            {
                if ((num & 1) == 1)
                {
                    int row = i * 16 + bitIndex / width;
                    int col = i * 16 + bitIndex - row * width;
                    map[row, col] = false;
                }
                num = (short)(num >> 1);
            }
        }
        return map;
    }

    public static void SaveBattleMapGrid(string sceneName, bool[,] map)
    {
        int bitCount = 0;
        StringBuilder hexUnitSb = new StringBuilder();
        StringBuilder hexStringSb = new StringBuilder();
        for (int i = 0; i < map.GetLength(0); ++i)
        {
            for (int j = 0; j < map.GetLength(1); ++j)
            {
                hexUnitSb.Append(map[i, j] ? '1' : '0');
                ++bitCount;
                if (bitCount == 16)
                {
                    short hexNum = Convert.ToInt16(hexUnitSb.ToString(), 16);
                    hexStringSb.Append(hexNum);
                    hexUnitSb.Clear();
                    bitCount = 0;
                }
            }
        }
        Debug.Log(hexStringSb.ToString());
    }
    
    

}

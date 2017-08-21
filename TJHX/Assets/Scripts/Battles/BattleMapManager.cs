using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum BattleMapTileType
{
    FreeMove, Enemy, Friend, Unreachable
}

class BattleMapManager : MonoBehaviour
{
    private static BattleMapManager _instance;
    public static BattleMapManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private int mapWidth = -1;
    private int mapHeight = -1;
    private BattleMapTileType[,] tileTypeInMap;

    private void Awake()
    {
        if (_instance != null)
        {
            DestroyImmediate(_instance.gameObject);
        }
        _instance = GetComponent<BattleMapManager>();
    }

    public void SetMapSize(int width, int height)
    {
        mapWidth = width;
        mapHeight = height;
    }

    public void Init()
    {
        if (mapWidth == -1 || mapHeight == -1)
        {
            Debug.LogError("初始化战斗地图管理器前应先设置地图大小");
        }
        tileTypeInMap = new BattleMapTileType[mapWidth, mapHeight];
        for (int i = 0; i < mapWidth; ++i)
        {
            for (int j = 0; j < mapHeight; ++j)
            {
                tileTypeInMap[i, j] = BattleMapTileType.FreeMove;
            }
        }
    }

    public void ShowMoveGrid(Character cht)
    {
        Debug.Log("cht.position: "+ cht.Position);
        int stepMax = cht.Speed / 10 + cht.BaseMove;
        Dictionary<Point, int> result = new Dictionary<Point, int>();
        BFSFindMoveGrid(cht.Position, 0, stepMax, result);
        foreach (var pair in result)
        {
            GameObject moveTileGO = Instantiate(Resources.Load("move_tile")) as GameObject;
            moveTileGO.transform.position = pair.Key.ToVector3();
        }
    }

    private void BFSFindMoveGrid(Point currentPos, int stepsMoved, int stepMax, Dictionary<Point, int> result)
    {
        if (!result.ContainsKey(currentPos))
            result.Add(currentPos, stepsMoved);
        else
            result[currentPos] = stepsMoved;
        if (stepsMoved == stepMax)
        {
            return;
        }
        var upPos = currentPos + Point.Up;
        var leftPos = currentPos + Point.Left;
        var downPos = currentPos + Point.Down;
        var rightPos = currentPos + Point.Right;
        if (IsMoveable(upPos) && (!result.ContainsKey(upPos) || (result[upPos] > stepsMoved + 1)))
        {
            BFSFindMoveGrid(upPos, stepsMoved + 1, stepMax, result);
        }
        if (IsMoveable(leftPos) && (!result.ContainsKey(leftPos) || (result[leftPos] > stepsMoved + 1)))
        {
            BFSFindMoveGrid(leftPos, stepsMoved + 1, stepMax, result);
        }
        if (IsMoveable(downPos) && (!result.ContainsKey(downPos) || (result[downPos] > stepsMoved + 1)))
        {
            BFSFindMoveGrid(downPos, stepsMoved + 1, stepMax, result);
        }
        if (IsMoveable(rightPos) && (!result.ContainsKey(rightPos) || (result[rightPos] > stepsMoved + 1)))
        {
            BFSFindMoveGrid(rightPos, stepsMoved + 1, stepMax, result);
        }

    }

    private bool IsMoveable(Point pos)
    {
        if (tileTypeInMap[pos.x, pos.y] == BattleMapTileType.FreeMove)
            return true;
        else
            return false;
    }

}
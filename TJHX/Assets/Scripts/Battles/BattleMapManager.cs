using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum BattleMapTileType
{
    FreeMove, Enemy, Friend, Unreachable
}

class BattleMapManager : Singleton<BattleMapManager>
{
    public Transform MoveTileContainer;
    public Transform ReachTileContainer;
    public Transform AttackTileContainer;


    private int mapWidth = -1;
    private int mapHeight = -1;
    private BattleMapTileType[,] tileTypeInMap;

    private List<GameObject> moveTileGOList;
    private List<GameObject> reachTileGOList;

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
        moveTileGOList = new List<GameObject>();
    }

    public void ClearMoveGrid()
    {
        //清除之前的移动范围GameObject
        if (moveTileGOList == null)
            return;
        foreach (var moveTileGo in moveTileGOList)
        {
            Destroy(moveTileGo);
        }
        moveTileGOList.Clear();
    }

    //显示移动范围
    public void ShowMoveGrid(Character cht)
    {
        ClearMoveGrid();
        //Debug.Log("cht.position: "+ cht.Position);
        uint stepMax = cht.Speed / 10 + cht.BaseMove;
        Dictionary<Point, uint> result = new Dictionary<Point, uint>();
        BFSFindMoveGrid(cht.Position, 0, stepMax, result);
        foreach (var pair in result)
        {
            //Debug.Log(pair);
            var moveTileGO = ResourcesManager.CreateByRid(R.Prefab.MoveTile, MoveTileContainer);
            moveTileGO.transform.position = pair.Key.ToVector3();
            moveTileGOList.Add(moveTileGO);
        }
    }

    private void BFSFindMoveGrid(Point currentPos, uint stepsMoved, uint stepMax, Dictionary<Point, uint> result)
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

    public void ShowWeaponReachRange(Character cht)
    {
        if (reachTileGOList == null)
        {
            reachTileGOList = new List<GameObject>();
        }
        Tool.ClearAndDestoryGO(reachTileGOList);
        Debug.Log(cht.Position.ToVector3());
        AttackTileContainer.position = cht.Position.ToVector3();
        cht.weaponArmed = new Weapon();
        cht.weaponArmed.AttackRange = new bool[,]
        {
            { true, true, true},
            { true, true, true}
        };
        int middlePos = cht.weaponArmed.AttackRange.GetLength(0) / 2 + 1;
        for (int i = 0; i < cht.weaponArmed.AttackRange.GetLength(0); ++i)
        {
            for (int j = 0; j < cht.weaponArmed.AttackRange.GetLength(1); ++j)
            {
                var reachGO = ResourcesManager.CreateByRid(R.Prefab.ReachTile, ReachTileContainer);
                if (cht.Direction == DirectionType.Up)
                    reachGO.transform.position = new Point(i - middlePos, j).ToVector3();
                else if (cht.Direction == DirectionType.Right)
                    reachGO.transform.position = new Point(j, i - middlePos).ToVector3();
                else if (cht.Direction == DirectionType.Down)
                    reachGO.transform.position = - new Point(i - middlePos, j).ToVector3();
                else if (cht.Direction == DirectionType.Left)
                    reachGO.transform.position = - new Point(j, i - middlePos).ToVector3();
                reachTileGOList.Add(reachGO);
            }
        }

        
    }

    public void HideWeaponReachRange()
    {
        Tool.ClearAndDestoryGO(reachTileGOList);
    }

}
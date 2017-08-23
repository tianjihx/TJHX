using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum BattleMapTileType
{
    FreeMove, Enemy, Friend, Unreachable
}

public class BattleMapManager : Singleton<BattleMapManager>
{
    public Transform MoveRangeContainer;
    public Transform AttackGroup;
    public Transform ReachRangeContainer;
    public Transform AttackRangeContainer;

    public Character characterWatching;

    public int mapWidth = 100;
    public int mapHeight = 100;
    private BattleMapTileType[,] tileTypeInMap;

    private List<GameObject> moveTileGOList;
    private List<GameObject> reachTileGOList;

    public void SetMapSize(int width, int height)
    {
        mapWidth = width;
        mapHeight = height;
    }

    public Point GetMapSize()
    {
        return new Point(mapWidth, mapHeight);
    }

    protected override void Awake()
    {
        base.Awake();
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

    public void SetMap(int x, int y, BattleMapTileType tileType)
    {
        tileTypeInMap[x, y] = tileType;
    }

    public BattleMapTileType GetMap(int x, int y)
    {
        return tileTypeInMap[x, y];
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
    public void ShowMoveRange()
    {
        if (!MoveRangeContainer.gameObject.activeSelf)
            MoveRangeContainer.gameObject.SetActive(true);
    }

    public void HideMoveRange()
    {
        if (MoveRangeContainer.gameObject.activeSelf)
            MoveRangeContainer.gameObject.SetActive(false);
    }

    public void GenerateMoveGrid()
    {
        ClearMoveGrid();
        //Debug.Log("cht.position: "+ cht.Position);
        uint stepMax = characterWatching.Speed / 10 + characterWatching.BaseMove;
        Dictionary<Point, uint> result = new Dictionary<Point, uint>();
        BFSFindMoveGrid(characterWatching.Position, 0, stepMax, result);
        foreach (var pair in result)
        {
            //Debug.Log(pair);
            var moveTileGO = ResourcesManager.CreateByRid(R.Prefab.MoveTile, MoveRangeContainer);
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

    public void ShowWeaponReachRange()
    {
        if (reachTileGOList == null)
        {
            reachTileGOList = new List<GameObject>();
        }
        if (reachTileGOList.Count != 0)
            return;
        Character cht = characterWatching;
        AttackGroup.position = cht.Position.ToVector3() + Tool.Direction2Point(cht.Direction).ToVector3();
        #region test_data
        cht.weaponArmed = new Weapon();
        cht.weaponArmed.AttackRange = new bool[,]
        {
            { true, true, true},
            { false, true, true}
        };
        #endregion
        int middlePos = cht.weaponArmed.AttackRange.GetLength(0) / 2;
        for (int i = 0; i < cht.weaponArmed.AttackRange.GetLength(0); ++i)
        {
            for (int j = 0; j < cht.weaponArmed.AttackRange.GetLength(1); ++j)
            {
                if (!cht.weaponArmed.AttackRange[i, j])
                    continue;
                var reachGO = ResourcesManager.CreateByRid(R.Prefab.ReachTile, ReachRangeContainer);
                if (cht.Direction == DirectionType.Up)
                    reachGO.transform.localPosition = new Point(middlePos - j, i).ToVector3();
                else if (cht.Direction == DirectionType.Right)
                    reachGO.transform.localPosition = new Point(i, j - middlePos).ToVector3();
                else if (cht.Direction == DirectionType.Down)
                    reachGO.transform.localPosition = - new Point(middlePos - j, i).ToVector3();
                else if (cht.Direction == DirectionType.Left)
                    reachGO.transform.localPosition = - new Point(i, j - middlePos).ToVector3();
                reachTileGOList.Add(reachGO);
            }
        }
    }

    public void HideWeaponReachRange()
    {
        if (reachTileGOList.Count == 0)
            return;
        Tool.ClearAndDestoryGO(reachTileGOList);
    }

    public void WatchCharacter(Character cht)
    {
        characterWatching = cht;
    }

    private void Update()
    {
        if (characterWatching == null)
            return;
        switch (characterWatching.status)
        {
            case CharacterStatus.Idle:
                ShowMoveRange();
                ShowWeaponReachRange();
                break;
            case CharacterStatus.Moving:
                HideWeaponReachRange();
                break;
            case CharacterStatus.ChooseTarget:
                HideMoveRange();
                break;
            case CharacterStatus.Attacking:
                break;
        }
    }

    #region DEBUG CODE

    public bool debug = false;

    void OnDrawGizmos()
    {
        if (debug)
        {

            for (int i = 0; i < mapWidth; ++i)
            {
                for (int j = 0; j < mapHeight; ++j)
                {
                    if (tileTypeInMap[i, j] == BattleMapTileType.Enemy)
                    {
                        GizmosDrawX(new Point(i, j), new Color(1.0f, 0, 0, 0.5f));
                    }
                }
            }
        }
    }

    void GizmosDrawX(Point pos, Color color)
    {
        Gizmos.color = color;
        Vector3 leftDown = pos.ToVector3() + new Vector3(-0.5f, 0, -1);
        Vector3 rightUp = pos.ToVector3() + new Vector3(0.5f, 0, 1);
        Vector3 leftUp = pos.ToVector3() + new Vector3(-0.5f, 0, 1);
        Vector3 rightDown = pos.ToVector3() + new Vector3(0.5f, 0, -1);
        Gizmos.DrawLine(rightUp, leftDown);
        Gizmos.DrawLine(leftUp, rightDown);

    }
    #endregion
}
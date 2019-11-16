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
    public Range MoveRange;
    public Range ReachRange;
    public Range AttackRange;
    public Transform AttackGroup;
    //public Transform ReachRangeContainer;
    //public Transform AttackRangeContainer;

    public Character chtWatching;

    private bool[,] obstacleMap;
    private Dictionary<Point, Character> characterMap;
    
    private List<GameObject> reachTileGOList;
    private List<GameObject> attackTileGOList;
    private Point startMovePosition;
    private Dictionary<Point, int> moveGridMap;
    

    public Point GetMapSize()
    {
        return new Point(obstacleMap.GetLength(0), obstacleMap.GetLength(1));
    }

    //protected override void Awake()
    //{
    //    base.Awake();
    //    characterMap = new Dictionary<Point, Character>();
    //}

    public Character GetCharacter(Point pos)
    {
        return GetCharacter(pos.x, pos.y);
    }

    public Character GetCharacter(int x, int y)
    {
        Character cht;
        if (characterMap.TryGetValue(new Point(x, y), out cht))
        {
            return cht;
        }
        else
        {
            return null;
        }
    }

    public int MoveSteps
    {
        get
        {
            return Point.Distance(chtWatching.Position, startMovePosition);
        }
    }

    public Point StartMovePosition
    {
        get
        {
            return startMovePosition;
        }
    }

    public Dictionary<Point, int> MoveGridMap
    {
        get
        {
            return moveGridMap;
        }
    }

    public void LoadMap(bool[,] map)
    {
        obstacleMap = map;
    }

    public bool GetMap(int x, int y)
    {
        return obstacleMap[x, y];
    }

    public int MapWidth { get { return obstacleMap.GetLength(0); } }
    public int MapHeight { get { return obstacleMap.GetLength(1); } }

    //显示移动范围
    public void ShowMoveRange()
    {
        MoveRange.Show();
    }
    
    //隐藏移动范围
    public void HideMoveRange()
    {
        MoveRange.Hide();
    }

    //根据检测的character生成移动范围
    public void GenerateMoveGrid()
    {
        if (moveGridMap == null)
            moveGridMap = new Dictionary<Point, int>();
        else
            moveGridMap.Clear();
        MoveRange.Clear();
        
        BFSFindMoveGrid(chtWatching.Position, 0, chtWatching.StepMax, moveGridMap);
        foreach (var pair in moveGridMap)
        {
            MoveRange.Add(pair.Key.x, pair.Key.y, Space.World);
        }
    }

    /// <summary>
    /// 宽搜地图，寻找能到达的点
    /// </summary>
    /// <param name="currentPos">当前搜索的点</param>
    /// <param name="stepsMoved">已经移动的步数</param>
    /// <param name="stepMax">最大可以移动的步数</param>
    /// <param name="result">保存能到达的点的集合</param>
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

    //判断指定的pos是否没有障碍物并且没有别的角色可以到达
    public bool IsMoveable(Point pos)
    {
        if (!obstacleMap[pos.x, pos.y] && GetCharacter(pos) == null)
            return true;
        else
            return false;
    }

    //根据检测的character生成武器选择范围
    public void GenerateWeaponReachRange()
    {
        ReachRange.Load(chtWatching.weaponArmed.ReachRange);
        ReachRange.LocalPosition = - chtWatching.weaponArmed.ReachCenter;
    }

    public void ShowWeaponReachRange()
    {
        ReachRange.Show();
    }

    public void HideWeaponReachRange()
    {
        ReachRange.Hide();
    }

    public void GenerateAttackRange()
    {
        AttackRange.Load(chtWatching.weaponArmed.AttackRange);
    }

    public void ShowAttackRange()
    {
        AttackRange.Show();
    }

    public void HideAttackRange()
    {
        AttackRange.Hide();
    }

    public void WatchCharacter(Character cht)
    {
        chtWatching = cht;
        startMovePosition = cht.Position;
    }

    private void Update()
    {
        if (chtWatching == null)
            return;
        switch (chtWatching.status)
        {
            case CharacterStatus.Idle:
                ShowMoveRange();
                ShowWeaponReachRange();
                HideAttackRange();
                AttackGroup.transform.position = chtWatching.Position.ToVector3();
                AttackGroup.transform.rotation = Tool.Direction2Rotation(chtWatching.Direction);
                break;
            case CharacterStatus.Moving:
                HideWeaponReachRange();
                break;
            case CharacterStatus.ChooseTarget:
                HideMoveRange();
                ShowAttackRange();
                AttackRange.LocalPosition = chtWatching.AimTarget - chtWatching.weaponArmed.AttackCenter;
                break;
            case CharacterStatus.Attacking:
                HideAttackRange();
                HideWeaponReachRange();
                break;
        }
    }

    #region DEBUG CODE

    public bool debug = false;

    void OnDrawGizmos()
    {
        if (debug)
        {
            
        }
    }

    void GizmosDrawX(Point pos, Color color)
    {
        Gizmos.color = color;
        Vector3 leftDown = pos.ToVector3() + new Vector3(-0.5f, 0, -0.5f);
        Vector3 rightUp = pos.ToVector3() + new Vector3(0.5f, 0, 0.5f);
        Vector3 leftUp = pos.ToVector3() + new Vector3(-0.5f, 0, 0.5f);
        Vector3 rightDown = pos.ToVector3() + new Vector3(0.5f, 0, -0.5f);
        Gizmos.DrawLine(rightUp, leftDown);
        Gizmos.DrawLine(leftUp, rightDown);

    }
    #endregion
}
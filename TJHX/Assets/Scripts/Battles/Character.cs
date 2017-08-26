using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public enum CharacterStatus
{
    Idle, Moving, ChooseTarget, ConfirmAttack, Attacking
}

public class Character : MonoBehaviour {

    public int HP;
    public int MP;
    public int EP;
    public int Attack;
    public int Defend;
    public int Agility;
    public int Intelligence;
    public int BaseMove;
    public int Speed;
    public int Luck;

    public Weapon weaponArmed;

    public CharacterStatus status;

    public Point AimTarget;

    private bool moveTargetRangeCD;
    

    #region Properties
    private Point _position;
    public Point Position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
            status = CharacterStatus.Moving;
            transform
                .DOLocalMove(_position.ToVector3(transform.position.y), 0.2f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    status = CharacterStatus.Idle;
                });
        }
    }

    private DirectionType _direction;
    public DirectionType Direction
    {
        get
        {
            return _direction;
        }
        set
        {
            _direction = value;
            transform.rotation = Tool.Direction2Rotation(_direction);
        }
    }

    public int StepMax
    {
        get
        {
            return Speed / 10 + BaseMove;
        }
    }
    #endregion

    private void Awake()
    {
        _position = new Point(transform.position);
        _direction = DirectionType.Down;
        moveTargetRangeCD = false;
    }

    

    public void MoveStep(DirectionType direction)
    {
        if (status != CharacterStatus.Idle)
            return;
        if (Direction != direction)
        {
            TurnTo(direction);
            return;
        }
        Point moveDelta = Point.Zero;
        switch (direction)
        {
            case DirectionType.Left:
                moveDelta = Point.Left;
                break;
            case DirectionType.Up:
                moveDelta = Point.Up;
                break;
            case DirectionType.Right:
                moveDelta = Point.Right; 
                break;
            case DirectionType.Down:
                moveDelta = Point.Down;
                break;
        }
        Point expectPosition = Position + moveDelta;
        if (BattleMapManager.Instance.IsMoveable(expectPosition) &&
            BattleMapManager.Instance.MoveGridMap.ContainsKey(expectPosition))
        {
            Position += moveDelta;
        }
    }


    public void TurnTo(DirectionType direction)
    {
        Direction = direction;
        status = CharacterStatus.Moving;
        Timer.New(() =>
        {
            Debug.Log("turn finish");
            status = CharacterStatus.Idle;
        }, 0.1f).Run();
    }

    public void MoveTo(Point destPos)
    {

    }

    public void ChooseTarget()
    {
        status = CharacterStatus.ChooseTarget;
        AimTarget = Point.Zero;
    }

    public void MoveTargetRange(DirectionType keyDirection)
    {
        if (moveTargetRangeCD)
            return;
        moveTargetRangeCD = true;
        Point localDirection = Point.Zero;
        switch (Direction)
        {
            case DirectionType.Down:
                switch (keyDirection)
                {
                    case DirectionType.Down: localDirection = Point.Up; break;
                    case DirectionType.Right: localDirection = Point.Left; break;
                    case DirectionType.Up: localDirection = Point.Down; break;
                    case DirectionType.Left: localDirection = Point.Right; break;
                }
                break;
            case DirectionType.Left:
                switch (keyDirection)
                {
                    case DirectionType.Down: localDirection = Point.Left; break;
                    case DirectionType.Right: localDirection = Point.Down; break;
                    case DirectionType.Up: localDirection = Point.Right; break;
                    case DirectionType.Left: localDirection = Point.Up; break;
                }
                break;
            case DirectionType.Up:
                switch (keyDirection)
                {
                    case DirectionType.Down: localDirection = Point.Down; break;
                    case DirectionType.Right: localDirection = Point.Right; break;
                    case DirectionType.Up: localDirection = Point.Up; break;
                    case DirectionType.Left: localDirection = Point.Left; break;
                }
                break;
            case DirectionType.Right:
                switch (keyDirection)
                {
                    case DirectionType.Down: localDirection = Point.Right; break;
                    case DirectionType.Right: localDirection = Point.Up; break;
                    case DirectionType.Up: localDirection = Point.Left; break;
                    case DirectionType.Left: localDirection = Point.Down; break;
                }
                break;
        }
        Point expectAimTarget = AimTarget + localDirection + weaponArmed.ReachCenter;
        var reachPointSet = BattleMapManager.Instance.ReachRange.GetPointSet();
        if (reachPointSet.Contains(expectAimTarget))
        {
            AimTarget += localDirection;
        }
        Timer.New(() =>
        {
            moveTargetRangeCD = false;
        }, 0.05f).Run();
    }

    public void WeaponAttack()
    {
        status = CharacterStatus.Attacking;
        GetComponentInChildren<Animator>().SetTrigger("Attack");
    }

    public void BackCommand()
    {
        if (status == CharacterStatus.ChooseTarget)
        {
            status = CharacterStatus.Idle;
        }
    }
}

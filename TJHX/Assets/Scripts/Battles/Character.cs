using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class Character : MonoBehaviour {

    public uint HP;
    public uint MP;
    public uint EP;
    public uint Attack;
    public uint Defend;
    public uint Agility;
    public uint Intelligence;
    public uint BaseMove;
    public uint Speed;
    public uint Luck;

    public Weapon weaponArmed;

    public bool IsMoving = false;

    

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
            IsMoving = true;
            transform
                .DOLocalMove(new Vector3(_position.x, transform.position.y, _position.y * 2), 0.2f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    BattleMapManager.Instance.ShowWeaponReachRange(this);
                    IsMoving = false;
                });
            //transform.position = new Vector3(_position.x, transform.position.y, _position.y);
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
    #endregion

    private void Awake()
    {
        _position = new Point(transform.position);
        _direction = DirectionType.Down;
    }

    

    public void MoveStep(DirectionType direction)
    {
        if (IsMoving)
            return;
        if (Direction != direction)
        {
            TurnTo(direction);
            return;
        }
        BattleMapManager.Instance.HideWeaponReachRange();
        switch (direction)
        {
            case DirectionType.Left:
                Position += Point.Left;
                break;
            case DirectionType.Up:
                Position += Point.Up;
                break;
            case DirectionType.Right:
                Position += Point.Right; 
                break;
            case DirectionType.Down:
                Position += Point.Down;
                break;
        }
        
    }


    public void TurnTo(DirectionType direction)
    {
        BattleMapManager.Instance.HideWeaponReachRange();
        Direction = direction;
        IsMoving = true;
        Timer.New(() =>
        {
            BattleMapManager.Instance.ShowWeaponReachRange(this);
            Debug.Log("turn finish");
            IsMoving = false;
        }, 0.2f).Run();
    }

    public void MoveTo(Point destPos)
    {

    }
}

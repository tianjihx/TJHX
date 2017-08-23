using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public enum CharacterStatus
{
    Idle, Moving, ChooseTarget, Attacking
}

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

    public CharacterStatus status;

    

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
    #endregion

    private void Awake()
    {
        _position = new Point(transform.position);
        _direction = DirectionType.Down;
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
}

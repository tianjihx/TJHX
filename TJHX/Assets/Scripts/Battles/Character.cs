using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

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
        IsMoving = true;
        Timer.New(() =>
        {
            Debug.Log("turn finish");
            IsMoving = false;
        }, 0.2f).Run();
    }

    public void MoveTo(Point destPos)
    {

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public struct Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Point(Vector2 v)
    {
        this.x = (int)v.x - 1;
        this.y = (int)(v.y) - 1;
    }

    public Point(Vector3 v)
    {
        this.x = (int)v.x - 1;
        this.y = (int)(v.z) - 1;
    }

    public Point(Vector3 v, bool withOffset)
    {
        if (withOffset)
        {
            this.x = (int)v.x - 1;
            this.y = (int)(v.z) - 1;
        }
        else
        {
            this.x = (int)v.x;
            this.y = (int)(v.z);
        }
        
    }

    public readonly static Point Up = new Point(0, 1);
    public readonly static Point Left = new Point(-1, 0);
    public readonly static Point Down = new Point(0, -1);
    public readonly static Point Right = new Point(1, 0);
    public readonly static Point Zero = new Point(0, 0);

    public static Point operator +(Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y);
    }

    public static Point operator -(Point a)
    {
        return new Point(-a.x, -a.y);
    }

    public static Point operator -(Point a, Point b)
    {
        return new Point(a.x - b.x, a.y - b.y);
    }

    public static Point operator *(Point a, int b)
    {
        return new Point(a.x * b, a.y * b);
    }

    public static Point operator /(Point a, int b)
    {
        return new Point(a.x / b, a.y / b);
    }

    public override string ToString()
    {
        return string.Format("({0},{1})", x, y);
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x + 1, 0, (y + 1));
    }

    public Vector3 ToVector3(float axisY)
    {
        return new Vector3(x + 1, axisY, (y + 1));
    }

    public Vector3 ToVector3WithoutOffset()
    {
        return new Vector3(x, 0, y);
    }

    public static int Distance(Point p1, Point p2)
    {
        return Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.y - p2.y);
    }
}

public enum DirectionType
{
    Left, Up, Right, Down
}
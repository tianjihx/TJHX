using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Range : MonoBehaviour
{
    [SerializeField] private GameObject m_TileTemplate;
    [SerializeField] private Transform m_Container;
    

    private List<GameObject> tiles;
    private HashSet<Point> tileLocalPositionSet;

    public Point Position
    {
        get
        {
            return new Point(transform.position);
        }
        set
        {
            transform.position = value.ToVector3();
        }
    }

    public Point LocalPosition
    {
        get
        {
            return new Point(transform.localPosition, false);
        }
        set
        {
            transform.localPosition = value.ToVector3WithoutOffset();
        }
    }

    public HashSet<Point> GetPointSet()
    {
        return tileLocalPositionSet;
    }

    /// <summary>
    /// 加载一个二维bool数组，描述了range的向下形状
    /// </summary>
    /// <param name="rangeMap"></param>
    public void Load(bool[,] rangeMap)
    {
        Tool.EnsureNotNull(ref tiles);
        Tool.EnsureNotNull(ref tileLocalPositionSet);
        Tool.ClearAndDestoryGO(tiles);
        tileLocalPositionSet.Clear();
        for (int y = rangeMap.GetLength(0) - 1; y >= 0; --y)
        {
            for (int x = 0; x < rangeMap.GetLength(1); ++x)
            {
                if (rangeMap[y, x])
                {
                    var tile = GameObject.Instantiate(m_TileTemplate);
                    tile.transform.SetParent(m_Container);
                    Point localPosition = new Point(x, rangeMap.GetLength(0) - y - 1);
                    tile.transform.localPosition = localPosition.ToVector3WithoutOffset();
                    tiles.Add(tile);
                    tileLocalPositionSet.Add(localPosition);
                }
            }
        }
    }

    public void Clear()
    {
        Tool.ClearAndDestoryGO(tiles);
    }

    public void Add(int x, int y, Space space)
    {
        if (tiles == null)
            tiles = new List<GameObject>();
        var tile = GameObject.Instantiate(m_TileTemplate);
        tile.transform.SetParent(m_Container);
        if (space == Space.Self)
            tile.transform.localPosition = new Point(x, y).ToVector3WithoutOffset();
        else if (space == Space.World)
            tile.transform.position = new Point(x, y).ToVector3();
        tiles.Add(tile);
    }

    public void Show()
    {
        m_Container.gameObject.SetActive(true);
    }

    public void Hide()
    {
        m_Container.gameObject.SetActive(false);
    }
}

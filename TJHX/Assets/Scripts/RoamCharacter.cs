using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RoamCharacter : MonoBehaviour
{
    private List<DirectionType> directionList;

    private void Awake()
    {
        directionList = new List<DirectionType>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            directionList.Add(DirectionType.Up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            directionList.Add(DirectionType.Down);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            directionList.Add(DirectionType.Left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            directionList.Add(DirectionType.Right);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            directionList.Remove(DirectionType.Up);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            directionList.Remove(DirectionType.Down);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            directionList.Remove(DirectionType.Left);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            directionList.Remove(DirectionType.Right);
        }

        if (directionList.Count > 0)
        {
            transform.position += Tool.Direction2Point(directionList[directionList.Count - 1]).ToVector3WithoutOffset() * Time.deltaTime * 5;
            TurnTo(directionList[directionList.Count - 1]);
        }
    }

    public void TurnTo(DirectionType direction)
    {
        transform.rotation = Tool.Direction2Rotation(direction);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour {

    public Character character;
    public string skillName;
    public Vector3 desPosition;
    public Transform desMark;

	public void OnTestSkillClick()
    {
        SkillLoader.Load(skillName);
    }

    

    public void OnShowMoveGridBtn()
    {
        BattleMapManager.Instance.ClearMoveGrid();
        BattleMapManager.Instance.SetMapSize(100, 100);
        BattleMapManager.Instance.Init();
        BattleMapManager.Instance.SetMap(49, 8, BattleMapTileType.Enemy);
        BattleMapManager.Instance.WatchCharacter(character);
        BattleMapManager.Instance.GenerateMoveGrid();
    }

    public void OnTestDbClick()
    {
        var a = DB.GetTestData();
        Debug.Log(a.a);
    }
}

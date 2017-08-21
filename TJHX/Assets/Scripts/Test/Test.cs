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
        
        BattleMapManager.Instance.SetMapSize(100, 100);
        BattleMapManager.Instance.Init();
        BattleMapManager.Instance.ShowMoveGrid(character);
    }
}

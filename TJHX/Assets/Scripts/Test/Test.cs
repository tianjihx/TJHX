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
        character.weaponArmed = new Weapon();
        character.weaponArmed.ReachRange = new bool[,]
        {
            { true, true, true},
            { false, true, true}
        };
        character.weaponArmed.AttackRange = new bool[,]
        {
            { false, true, false},
            { true, true, true},
            { false, true, false},
        };
        character.weaponArmed.AttackCenter = new Point(1, 1);
        character.weaponArmed.ReachCenter = new Point(1, 0);

        BattleMapManager.Instance.LoadMap(DB.LoadBattleMapGrid("TestScene"));
        BattleController.Instance.Cam.Watch(character);
        BattleMapManager.Instance.WatchCharacter(character);
        BattleMapManager.Instance.GenerateMoveGrid();
        BattleMapManager.Instance.GenerateWeaponReachRange();
        BattleMapManager.Instance.GenerateAttackRange();
    }

    public void OnTestDbClick()
    {
        var a = DB.GetTestData();
        Debug.Log(a.a);
    }
}

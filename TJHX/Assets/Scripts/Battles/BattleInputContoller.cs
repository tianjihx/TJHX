using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInputContoller : MonoBehaviour {

    public Character heroCurrentControl;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (heroCurrentControl == null)
            return;
        if (heroCurrentControl.status == CharacterStatus.Idle)
        {
            if (Input.GetKey(KeyCode.DownArrow))
                heroCurrentControl.MoveStep(DirectionType.Down);
            else if (Input.GetKey(KeyCode.LeftArrow))
                heroCurrentControl.MoveStep(DirectionType.Left);    
            else if (Input.GetKey(KeyCode.UpArrow))
                heroCurrentControl.MoveStep(DirectionType.Up);    
            else if (Input.GetKey(KeyCode.RightArrow))
                heroCurrentControl.MoveStep(DirectionType.Right);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                heroCurrentControl.ChooseTarget();
            }
        }
        else if (heroCurrentControl.status == CharacterStatus.ChooseTarget)
        {
            if (Input.GetKey(KeyCode.DownArrow))
                heroCurrentControl.MoveTargetRange(DirectionType.Down);
            else if (Input.GetKey(KeyCode.LeftArrow))
                heroCurrentControl.MoveTargetRange(DirectionType.Left);
            else if (Input.GetKey(KeyCode.UpArrow))
                heroCurrentControl.MoveTargetRange(DirectionType.Up);
            else if (Input.GetKey(KeyCode.RightArrow))
                heroCurrentControl.MoveTargetRange(DirectionType.Right);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                heroCurrentControl.WeaponAttack();
            }
        }

        


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            heroCurrentControl.BackCommand();
        }
    }
}

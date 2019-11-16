using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TJHX
{
    class InputManager
    {
        public Vector2 LeftAxis;

        public void Update()
        {
            ModifyOnKeyDown(KeyCode.W, ref LeftAxis.y, 1);
            ModifyOnKeyDown(KeyCode.A, ref LeftAxis.x, -1);
            ModifyOnKeyDown(KeyCode.S, ref LeftAxis.y, -1);
            ModifyOnKeyDown(KeyCode.D, ref LeftAxis.x, 1);
            ModifyOnKeyUp(KeyCode.W, ref LeftAxis.y, -1);
            ModifyOnKeyUp(KeyCode.A, ref LeftAxis.x, 1);
            ModifyOnKeyUp(KeyCode.S, ref LeftAxis.y, 1);
            ModifyOnKeyUp(KeyCode.D, ref LeftAxis.x, -1);
        }

        public void ModifyOnKeyDown(KeyCode key, ref float target, float value)
        {
            if (Input.GetKeyDown(key))
            {
                target += value;
            }
        }

        public void ModifyOnKeyUp(KeyCode key, ref float target, float value)
        {
            if (Input.GetKeyUp(key))
            {
                target += value;
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TJHX
{
    /// <summary>
    /// Actor的基础属性，包含基本的显示信息，移动信息
    /// </summary>
    class Property : MonoBehaviour
    {
        public Actor Actor;
        public string Name;                //显示名称

        public Vector2 Position;
        public Direction FaceDirection;    //面向方向
        public bool IsMove;                //是否移动
        public Direction MoveDirection;    //移动方向
        public float MoveSpeed;             //移动速度

        private void Awake()
        {
            Actor = GetComponent<Actor>();
        }

        private void Update()
        {
            //如果操作的是自己，读取input
            if (ActorManager.Instance.ControlActor == Actor)
            {
                MoveDirection.value = GameEntry.InputManager.LeftAxis;
                if (MoveDirection.value != Vector2.zero)
                    FaceDirection = MoveDirection;
            }
            if (IsMove)
            {
                Position += MoveDirection.value.normalized * MoveSpeed * Time.deltaTime;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TJHX
{
    class Actor : MonoBehaviour
    {
        /// <summary>
        /// 全局标识符，保证唯一
        /// </summary>
        public ulong Gid;
        public Property Property;

        private static Vector3 tempPosition;

        private void Awake()
        {
            Gid = 0;
        }

        private void Update()
        {
            tempPosition.x = Property.Position.x;
            tempPosition.y = 0;
            tempPosition.z = Property.Position.y;
            transform.position = tempPosition;
            transform.rotation = Property.FaceDirection.ToRotation();
        }
    }
}

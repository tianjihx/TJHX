using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TJHX
{
    [Serializable]
    struct Direction
    {
        [SerializeField]
        public Vector2 value;
        public float x
        {
            get { return value.x; }
            set { this.value.x = value; }
        }
        public float y
        {
            get { return value.y; }
            set { this.value.y = value; }
        }

        public Direction(float x, float y)
        {
            value = new Vector2(x, y);
        }

        public static readonly Direction None = new Direction(0, 0);
        public static readonly Direction North = new Direction(0, 1);
        public static readonly Direction East = new Direction(1, 0);
        public static readonly Direction South = new Direction(0, -1);
        public static readonly Direction West = new Direction(-1, 0);

        public Quaternion ToRotation()
        {
            return Quaternion.Euler(0, Vector2.SignedAngle(Vector2.up, value), 0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TJHX
{
    class ActorManager : Singleton<ActorManager>
    {
        private Actor _controlActor;

        public ActorManager()
        {

        }

        public Actor ControlActor
        {
            get
            {
                return _controlActor;
            }
            set
            {
                _controlActor = value;
            }
        }


    }
}

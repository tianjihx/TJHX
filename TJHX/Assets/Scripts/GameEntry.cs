using UnityEngine;
using System.Collections;

namespace TJHX
{
    class GameEntry : MonoBehaviour
    {
        private static GameEntry _instance;
        public static GameEntry Instance
        {
            get
            {
                if (_instance == null)
                {
                    var gameEntryGo = GameObject.Find("GameEntry");
                    if (gameEntryGo == null)
                    {
                        Debug.LogError("没有找到GameEntry对象");
                    }
                    _instance = gameEntryGo.GetComponent<GameEntry>();
                    if (gameEntryGo == null)
                    {
                        Debug.LogError("没有找到GameEntry脚本");
                    }
                }
                return _instance;
            }
        }

        private InputManager _InputManager;
        public static InputManager InputManager => Instance._InputManager;

        private void Awake()
        {
            _InputManager = new InputManager();
        }

        // Use this for initialization
        void Start()
        {
            ActorManager.Instance.ControlActor = GameObject.Find("TestActor").GetComponent<Actor>();
        }

        // Update is called once per frame
        void Update()
        {
            InputManager.Update();
        }

        private void FixedUpdate()
        {
        }
    }

}
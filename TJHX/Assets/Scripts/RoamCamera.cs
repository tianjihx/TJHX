using UnityEngine;
using System.Collections;

namespace TJHX
{
    class RoamCamera : MonoBehaviour
    {
        private Camera cam;
        public Actor m_WatchingActor;

        private void Awake()
        {
            cam = GetComponentInChildren<Camera>();
            if (cam == null)
            {
                Debug.Log("没有找到漫游相机！");
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        private void Update()
        {
            if (ActorManager.Instance.ControlActor != m_WatchingActor)
            {
                m_WatchingActor = ActorManager.Instance.ControlActor;
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (ActorManager.Instance.ControlActor != null)
            {
                transform.position = m_WatchingActor.transform.position;
            }
        }
    }

}
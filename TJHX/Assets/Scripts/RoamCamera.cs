using UnityEngine;
using System.Collections;

public class RoamCamera : MonoBehaviour
{
    private Camera cam;
    public Transform WatchTransform;

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

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = WatchTransform.position;
    }
}

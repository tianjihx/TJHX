using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class BattleCamera : MonoBehaviour {

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    // Use this for initialization
    void Start () {
        //cam.projectionMatrix = cam.projectionMatrix * Matrix4x4.Scale(new Vector3(0.5f, 1.0f, 1.0f));
	}

    // Update is called once per frame
    void Update () {
		
	}
}

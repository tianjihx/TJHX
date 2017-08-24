using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour {

    private Camera cam;
    private Character characterWahcing;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Use this for initialization
    void Start () {
        //cam.projectionMatrix = cam.projectionMatrix * Matrix4x4.Scale(new Vector3(0.5f, 1.0f, 1.0f));
	}

    // Update is called once per frame
    void LateUpdate () {
        if (characterWahcing == null)
            return;
        transform.position = characterWahcing.transform.position;
    }

    public void Watch(Character cht)
    {
        characterWahcing = cht;
    }
}

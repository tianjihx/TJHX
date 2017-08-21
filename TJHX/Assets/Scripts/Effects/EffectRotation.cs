using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRotation : MonoBehaviour {

    [Header("每秒钟旋转的角度")]
    public float rotationSpeed = 180.0f;
    public bool isEllipse = true;
    public float a = 0.4f;
    public float b = 0.6f;

    private float totalRotaionAngle = 0.0f;
    private Vector3 posDelta;
    private Vector3 originPos;

    private void Awake()
    {
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update () {
        totalRotaionAngle -= rotationSpeed * Time.deltaTime;
        totalRotaionAngle -= (int)(totalRotaionAngle / 360) * 360;
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        if (isEllipse)
        {
            posDelta = new Vector3((b-a) * Mathf.Cos(totalRotaionAngle / 180 * Mathf.PI), 0, 0);
            transform.position = originPos + posDelta;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

class Tool
{
    private readonly static Quaternion LeftRotation = Quaternion.Euler(new Vector3(0, 90, 0));
    private readonly static Quaternion RightRotation = Quaternion.Euler(new Vector3(0, 180, 0));
    private readonly static Quaternion UpRotation = Quaternion.Euler(new Vector3(0, 270, 0));
    private readonly static Quaternion DownRotation = Quaternion.Euler(new Vector3(0, 0, 0));

    public static Quaternion Direction2Rotation(DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.Left:
                return LeftRotation;
            case DirectionType.Up:
                return RightRotation;
            case DirectionType.Right:
                return UpRotation;
            case DirectionType.Down:
                return DownRotation;
        }
        return new Quaternion();
    }

    public static Point Direction2Point(DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.Left:
                return Point.Left;
            case DirectionType.Up:
                return Point.Up;
            case DirectionType.Right:
                return Point.Right;
            case DirectionType.Down:
                return Point.Down;
        }
        return Point.Zero;
    }

    public static void ClearAndDestoryGO<T>(List<T> list) where T : MonoBehaviour
    {
        foreach (T t in list)
        {
            GameObject.Destroy(t.gameObject);
        }
    }

    public static void ClearAndDestoryGO(List<GameObject> list)
    {
        if (list == null)
            return;
        foreach (var go in list)
        {
            GameObject.Destroy(go);
        }
    }
}

class Timer : MonoBehaviour
{
    private UnityAction OnComplete;
    private float pastTime;
    private float delay;
    private bool isRepeat;
    private bool isTick;

    private void Awake()
    {
        isTick = false;
        pastTime = 0;
    }

    public static Timer New(UnityAction action, float delay, bool repeat = false)
    {
        Timer timer = new GameObject("Timer").AddComponent<Timer>();
        timer.gameObject.hideFlags = HideFlags.HideAndDontSave;
        timer.OnComplete = action;
        timer.delay = delay;
        timer.isRepeat = repeat;
        return timer;
    }

    private void Update()
    {
        if (!isTick)
            return;
        pastTime += Time.deltaTime;
        if (pastTime > delay)
        {
            OnComplete();
            if (!isRepeat)
            {
                DestroyImmediate(gameObject);
                return;
            }
            pastTime -= delay;
        }
    }

    public void Run()
    {
        isTick = true;
    }

    public void Pause()
    {
        isTick = false;
    }

    public void Stop()
    {
        Pause();
        pastTime = 0;
    }

    public void Complete()
    {
        OnComplete();
        if (!isRepeat)
        {
            DestroyImmediate(gameObject);
            return;
        }
        pastTime = 0;
    }

    public void Cancel()
    {
        DestroyImmediate(gameObject);
    }
    


}

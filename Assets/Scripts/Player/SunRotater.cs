using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotater : MonoBehaviour
{
    [SerializeField]
    private float _start;

    [SerializeField]
    private float _end;

    public void RotateSun(float value)
    {
        var angle = Mathf.Lerp(_end, _start, 1 - value);
        if (angle > 180)
            angle -= 360;
        if (angle < 180)
            angle += 360;
        var euler = this.transform.eulerAngles;
        euler.x = angle;
        this.transform.eulerAngles = euler;
    }
}

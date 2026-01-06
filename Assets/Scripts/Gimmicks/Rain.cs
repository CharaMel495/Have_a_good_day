using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : GimmickBase
{
    public override void Initialize(Transform target)
    {
        var vec = target.position - this.transform.position;
        _rb = this.GetComponent<Rigidbody>();
        _timer = new();
    }

    public override void Shed(Vector3 impactNormal)
    {

    }
}

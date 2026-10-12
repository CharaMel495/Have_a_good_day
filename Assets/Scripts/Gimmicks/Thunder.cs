using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : GimmickBase
{
    [SerializeField]
    private GroundWalker _gWalker;

    public override void Initialize(Transform target)
    {
        _timer = new();
        _timer.Initialize();

        _rb = this.GetComponent<Rigidbody>();

        _gWalker.Target = target;
        _gWalker.MoveSpeed = _moveSpeed;

        EventDispatcher.Instance.Subscribe("EndRain",
            (_) =>
            {
                if (this == null)
                    return;

                if (this.gameObject != null)
                    Destroy(this.gameObject);
            });

        EventDispatcher.Instance.Subscribe("OnEnterResultMode",
            (_) =>
            {
                if (this == null)
                    return;

                if (this.gameObject != null)
                    Destroy(this.gameObject);
            });
    }

    private void FixedUpdate()
    {
        _timer.Update();

        if (!IsSheded)
            _gWalker.Walk();
    }

    public override void Shed(Vector3 impactNormal)
    {

    }
}

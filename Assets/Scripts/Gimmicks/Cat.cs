using CriWare;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : GimmickBase
{
    [SerializeField]
    private GroundWalker _gWalker;

    [SerializeField]
    private CriAtomSource _atomSource;

    private float _soundInterval = 0.4f;

    public override void Initialize(Transform target)
    {
        _timer = new();
        _timer.Initialize();

        _timer.CreateTask(PlayWalkSound, _soundInterval);

        _rb = this.GetComponent<Rigidbody>();

        _gWalker.Target = target;
        _gWalker.MoveSpeed = _moveSpeed;
    }

    private void FixedUpdate()
    {
        _timer.Update();

        if (!IsSheded)
            _gWalker.Walk();
    }

    private void PlayWalkSound()
    {
        if (this.gameObject == null)
            return;

        CRISoundManager.Instance.PlaySE(SFX.CatWalk, _atomSource);
        _timer.CreateTask(PlayWalkSound, _soundInterval);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bamboo : GimmickBase
{
    [SerializeField]
    private Transform _bambooShort;
    [SerializeField]
    private Transform _bamboo;

    [SerializeField]
    private float _appearHeight;
    [SerializeField]
    private float _pumpUpHeight;

    [SerializeField]
    private float _growTime;

    private bool _isGrowed;

    private const float _INITIALIZEDELAY = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        _timer = new();
        _timer.CreateTask(() => Initialize(null), _INITIALIZEDELAY);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _timer.Update();
    }

    public override void Initialize(Transform target)
    {
        EventDispatcher.Instance.Bind(this);
        _rb = this.GetComponent<Rigidbody>();
        _isGrowed = false;
    }

    public override void Shed(Vector3 impactNormal)
    {
        if (_isGrowed)
            base.Shed(impactNormal);
    }

    private void Appear()
    {
        _bambooShort.DOMoveY(_appearHeight, _growTime).SetEase(Ease.OutCubic);
    }

    private void StartGrowth()
    {
        _bambooShort.DOMoveY(-_appearHeight, _growTime);
        _bamboo.DOMoveY(_pumpUpHeight, _growTime).SetEase(Ease.OutCubic).OnComplete(UnLock);
    }

    private void UnLock()
    {
        _isGrowed = true;
        _rb.freezeRotation = false;
    }

    [CallableEvent("StartRain")]
    public void OnRainStart(object data)
    {
        Appear();
    }

    [CallableEvent("EndRain")]
    public void OnRainEnd(object data)
    {
        StartGrowth();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valcan : MonoBehaviour
{
    [Header("参照したいもの")]
    [SerializeField]
    private Transform _shootingPosition;

    [SerializeField]
    private ParticleSystem _muzzleFlush;

    [SerializeField]
    private ParticleSystem _hitEffect;

    private const float _ROTATERATE = 360 / 60.0f;
    private float _spinedValue;

    [Header("パラメータ")]
    [SerializeField]
    private float _valcanTime;

    [SerializeField]
    private float _spinUpTime;

    [SerializeField]
    private float _maxSpinSpeed;

    private float _currentSpinSpeed;

    private bool _isShootable;

    private Durator _durator;

    private void Start()
    {
        _durator = new();
    }

    private void FixedUpdate()
    {
        _durator.Update();
        Spin();
    }

    public void Begin()
    {
        _durator.CreateTask(SpinUp, StartShooting, _spinUpTime);
    }

    private void StartShooting()
    {
        _isShootable = true;
        _currentSpinSpeed = _maxSpinSpeed;
        _durator.CreateTask(SlowDown, EndValcan, _valcanTime);
    }

    private void EndValcan()
    {
        _isShootable = false;
        _currentSpinSpeed = 0.0f;
        EventDispatcher.Instance.Dispatch("EndValcan");
    }

    private void Spin()
    {
        var euler = this.transform.eulerAngles;
        var addValue = _currentSpinSpeed;
        euler.z += addValue;
        this.transform.eulerAngles = euler;

        _spinedValue += addValue;

        if (_spinedValue < _ROTATERATE)
            return;

        if (_isShootable)
            Shoot();

        _spinedValue -= _ROTATERATE;
    }

    private void Shoot()
    {
        _muzzleFlush.Play();
        var hit = Physics.Raycast(_shootingPosition.position, -_shootingPosition.right, out RaycastHit hitObj);

        if (hit && hitObj.collider.gameObject.TryGetComponent(out GimmickBase gimmick))
        {
            gimmick.Shed(_shootingPosition.forward);
            Instantiate(_hitEffect, hitObj.point, Quaternion.identity);
        }
    }

    public void SpinUp(float elapsedTime, float endTime)
    {
        var t = Mathf.InverseLerp(0, endTime, elapsedTime);
        _currentSpinSpeed = Mathf.Lerp(0, _maxSpinSpeed, t);
    }

    public void SlowDown(float elapsedTime, float endTime)
    {
        var t = Mathf.InverseLerp(0, endTime, elapsedTime);
        _currentSpinSpeed = Mathf.Lerp(_maxSpinSpeed, 0.0f, t);
    }
}

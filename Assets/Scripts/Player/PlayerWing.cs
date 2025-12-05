using CriWare;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerWing : MonoBehaviour
{
    private PlayerWind _wind;
    // コントローラーを振ったか確認する
    private WingFlapDetector _flapChecker;
    // OnCollisionEnterを使うため
    private Rigidbody _rb;

    [SerializeField]
    private bool _isRightHand;

    [SerializeField]
    private float _falpInterval = 0.5f;

    private bool _isFlapable = true;

    private Timer _timer;

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        _flapChecker = new(this.transform);
        _wind = new();
        _isFlapable = true;
        _timer = new();
    }

    private void FixedUpdate()
    {
        _timer.Update();

        if (_isFlapable && _flapChecker.CheckFlap(out Vector3 vel))
        {
            _wind.ApplyWind(this.transform.position, _isRightHand ?
                _wind.GetRightWindDir(vel) :
                _wind.GetLeftWindDir(vel));

            _isFlapable = false;

            _timer.CreateTask(() => _isFlapable = true, _falpInterval);
        }

        _flapChecker.UpdatePos();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<GimmickBase>(out var gimmick))
        {
            gimmick.Shed(collision.contacts[0].normal.normalized);
        }
    }
}

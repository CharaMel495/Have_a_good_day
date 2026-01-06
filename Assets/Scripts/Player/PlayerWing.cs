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
    private Transform _playerTransform;

    [SerializeField]
    private bool _isRightHand;

    [SerializeField]
    private float _falpInterval = 0.5f;

    private bool _isFlapable = true;

    private Timer _timer;

    private enum FlapDirection
    {
        Vertical,
        Horizontal
    }

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
            if (GetFlapDirection(vel, _playerTransform.forward) == FlapDirection.Vertical)
            {
                EventDispatcher.Instance.Dispatch("PlayerFlight");
            }
            else
            {
                _wind.ApplyWind(this.transform.position, _isRightHand ?
                    _wind.GetRightWindDir(vel) :
                    _wind.GetLeftWindDir(vel));
            }

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

    private FlapDirection GetFlapDirection(Vector3 vel, Vector3 playerForward)
    {
        // 念のため正規化
        playerForward.y = 0f;
        playerForward.Normalize();

        Vector3 right = Vector3.Cross(Vector3.up, playerForward).normalized;

        // 各成分を取得
        float verticalPower = Mathf.Abs(Vector3.Dot(vel, Vector3.up));
        float horizontalPower = Mathf.Abs(Vector3.Dot(vel, right));

        return verticalPower > horizontalPower
            ? FlapDirection.Vertical
            : FlapDirection.Horizontal;
    }

}

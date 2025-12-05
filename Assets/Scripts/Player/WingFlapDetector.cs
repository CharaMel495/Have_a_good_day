using UnityEngine;

public class WingFlapDetector
{
    private Transform _controller;       // 片手コントローラー
    private float _flapThreshold = 4.5f; // 振り速度のしきい値

    private Vector3 _lastPos;

    public WingFlapDetector(Transform hand)
    {
        _controller = hand;
        UpdatePos();
    }

    public void UpdatePos()
        => _lastPos = _controller.position;

    public bool CheckFlap(out Vector3 vel)
    {
        vel = (_controller.position - _lastPos) / Time.fixedDeltaTime;

        return vel.magnitude > _flapThreshold;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchDetector : MonoBehaviour
{
    public Transform head;       // HMDカメラ
    public float standingHeight = 1.5f;
    public float crouchThreshold = 1.15f; // これ以下ならしゃがみ

    public bool IsCrouching { get; private set; }
    public float HeadHeight { get; private set; }

    private bool _crouchPrev;

    [SerializeField]
    private bool _isDebug = false;

    private float _debugHeight;
    private float _debugTime;

    void Update()
    {
        if (_isDebug)
            SetCrouchDebug();
        else
            SetCrouchHead();
    }

    private void SetCrouchHead()
    {
        HeadHeight = head.localPosition.y;

        IsCrouching = (HeadHeight < crouchThreshold);

        _crouchPrev = IsCrouching;
    }

    private void SetCrouchDebug()
    {
        if (!InputSystemManager.CheckActionPressed("DebugHeadMove", InputHandler.Player, true))
            return;

        _debugTime += Time.fixedDeltaTime;
        var t = Mathf.Cos(_debugTime) * 0.5f + 0.5f;
        HeadHeight = Mathf.Lerp(crouchThreshold, standingHeight, t);
    }
}
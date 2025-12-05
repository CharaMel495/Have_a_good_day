using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchDetector : MonoBehaviour
{
    public Transform head;       // HMDƒJƒƒ‰
    public float standingHeight = 1.5f;
    public float crouchThreshold = 1.15f; // ‚±‚êˆÈ‰º‚È‚ç‚µ‚á‚ª‚İ

    public bool IsCrouching { get; private set; }
    public float HeadHeight { get; private set; }

    private bool _crouchPrev;

    void Update()
    {
        HeadHeight = head.localPosition.y;

        IsCrouching = (HeadHeight < crouchThreshold);

        _crouchPrev = IsCrouching;
    }
}
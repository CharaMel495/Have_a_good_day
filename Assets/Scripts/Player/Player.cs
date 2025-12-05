using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CrouchDetector _crouchChecker;

    [SerializeField]
    private Transform _cameraTransform;

    [SerializeField]
    private EggPlant _eggPlant;

    [SerializeField]
    private Animator _bodyAnimator;
    [SerializeField]
    private Transform _bodyTransform;
    //private int _crouchFlagHash = Animator.StringToHash("IsCrouch");
    private int _crouchBlendHash = Animator.StringToHash("CrouchBlend");

    public void Initialize()
    {
        _eggPlant.Initialize();

        _crouchChecker = this.GetComponent<CrouchDetector>();
    }

    private void FixedUpdate()
    {
        if (_crouchChecker.IsCrouching)
            _eggPlant.Warm();

        _bodyAnimator.SetFloat(_crouchBlendHash, Mathf.InverseLerp(_crouchChecker.crouchThreshold, _crouchChecker.standingHeight, _crouchChecker.HeadHeight));
        
        // カメラの forward を水平成分だけ取り出す
        Vector3 camForward = _cameraTransform.forward;
        camForward.y = 0f;                          // 上下成分を消す
        camForward.Normalize();

        // モデルを水平 forward に向ける（Up は常に地面に垂直）
        _bodyTransform.rotation = Quaternion.LookRotation(camForward, Vector3.up);
    }
}

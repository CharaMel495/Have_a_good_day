using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CrouchDetector _crouchChecker;

    [SerializeField]
    private LaderCyrinder _laderPrefab;

    [SerializeField]
    private Transform _cameraTransform;

    [SerializeField]
    private EggPlant _eggPlant;

    [Header("体のパーツ")]
    [SerializeField]
    private Animator _bodyAnimator;
    [SerializeField]
    private Transform _bodyTransform;
    [SerializeField]
    private Transform _wingTransformR;
    [SerializeField]
    private Transform _wingTransformL;
    [SerializeField]
    private Vector3 _biggestScale;
    [SerializeField]
    private Vector3 _smallestScale;
    //private int _crouchFlagHash = Animator.StringToHash("IsCrouch");
    private int _crouchBlendHash = Animator.StringToHash("CrouchBlend");

    public void Initialize()
    {
        _eggPlant.Initialize();

        _crouchChecker = this.GetComponent<CrouchDetector>();

        EventDispatcher.Instance.Bind(this);
    }

    private void FixedUpdate()
    {
        _eggPlant.Warm(_crouchChecker.IsCrouching);

        _bodyAnimator.SetFloat(_crouchBlendHash, Mathf.InverseLerp(_crouchChecker.crouchThreshold, _crouchChecker.standingHeight, _crouchChecker.HeadHeight));
        
        // カメラの forward を水平成分だけ取り出す
        Vector3 camForward = _cameraTransform.forward;
        camForward.y = 0f;                          // 上下成分を消す
        camForward.Normalize();

        // モデルを水平 forward に向ける（Up は常に地面に垂直）
        _bodyTransform.rotation = Quaternion.LookRotation(camForward, Vector3.up);

        SetWingSize();
    }

    //[CallableEvent("SpawnEnemy")]
    //public void CreateLader(object data)
    //{
    //    if (data is GimmickBase gimmick)
    //    {
    //        var lader = Instantiate(_laderPrefab, this.transform.position, Quaternion.identity);

    //        lader.PlayerTransform = this.transform;
    //        lader.GimmickTransform = gimmick.transform;
    //    }
    //}

    private void SetWingSize()
    {
        var t = Mathf.InverseLerp(_crouchChecker.crouchThreshold, _crouchChecker.standingHeight, _crouchChecker.HeadHeight);
        var scale = Vector3.Lerp(_smallestScale, _biggestScale, t);
        _wingTransformR.localScale = scale;
        _wingTransformL.localScale = scale;
    }
}

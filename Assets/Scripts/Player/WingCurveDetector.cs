using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 羽を曲げるべきかを確認するクラス
/// </summary>
public class WingCurveDetector : MonoBehaviour
{
    [SerializeField]
    private Transform _vecRef;

    [SerializeField]
    private bool _isRightHand;

    [SerializeField]
    private Animator _wingAnimator;
    private int _wingCurveFalgHash = Animator.StringToHash("IsCurve");
    private int _wingCurveValueHash = Animator.StringToHash("CurveValue");

    public bool IsCurving
    { get; private set; }


    void Update()
    {
        IsCurving = IsControllerDiagonal();

        _wingAnimator.SetBool(_wingCurveFalgHash, IsCurving);
    }

    //bool IsControllerInFrontDiagonal()
    //{
    //    Vector3 controllerPos = this.transform.position;
    //    Vector3 forward = _vecRef.forward;
    //    Vector3 right = _vecRef.right;

    //    Vector3 toController = (controllerPos - _vecRef.position).normalized;

    //    float f = Vector3.Dot(forward, toController);
    //    float r = Vector3.Dot(right, toController);

    //    // 「前側」かつ「左右どちらか」→ 斜め前の判定
    //    // 例：右前を判定したいなら f>0 && r>0
    //    return _isRightHand ?
    //        (f > 0f) && (Mathf.Abs(r) > 0f) :
    //        (f > 0f) && (Mathf.Abs(r) < 0f);
    //}

    bool IsControllerDiagonal(float minAngle = 0f, float maxAngle = 30f)
    {
        // コントローラーの方向（水平だけ）
        Vector3 toController = this.transform.position - _vecRef.position;
        toController.y = 0f;
        toController.Normalize();

        // プレイヤー正面（水平だけ）
        Vector3 forward = _vecRef.forward;
        forward.y = 0f;
        forward.Normalize();

        float angle = Vector3.Angle(forward, toController);

        // 斜め前の角度範囲に入っているか？
        bool inDiagonalRange = angle >= minAngle && angle <= maxAngle;

        _wingAnimator.SetFloat(_wingCurveValueHash, Mathf.InverseLerp(maxAngle, minAngle, angle));

        if (!inDiagonalRange) return false;

        // 右判定 or 左判定
        float side = Vector3.Dot(_vecRef.right, toController);

        if (_isRightHand)
            // 右斜め前
            return side > 0f;
        else
            // 左斜め前
            return side < 0f;
    }
}

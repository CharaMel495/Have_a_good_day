using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWind
{
    public float windRange = 80f;
    public float windAngle = 45f;
    public float windPower = 20f;

    public ParticleSystem windEffect; // 羽ばたきのパーティクル

    public Vector3 GetRightWindDir(Vector3 move)
    {
        // 上下成分を削除して水平だけに
        move.y = 0f;

        // 動きが小さすぎるときは無風
        if (move.sqrMagnitude < 0.0001f)
            return Vector3.zero;

        move.Normalize();

        // 振り方向に対し “右側” の直角方向 = Cross(up, move)
        Vector3 windDir = Vector3.Cross(Vector3.up, move).normalized;

        return windDir;
    }

    public Vector3 GetLeftWindDir(Vector3 move)
    {
        // 上下成分を削除して水平だけに
        move.y = 0f;

        // 動きが小さすぎるときは無風
        if (move.sqrMagnitude < 0.0001f)
            return Vector3.zero;

        move.Normalize();

        // 振り方向に対し “左側” の直角方向 = Cross(move, up)
        Vector3 windDir = Vector3.Cross(move, Vector3.up).normalized;

        return windDir;
    }

    public void ApplyWind(Vector3 controllerPos, Vector3 windDir)
    {
        if (windDir == Vector3.zero)
            return;

        Collider[] hits = Physics.OverlapSphere(controllerPos, windRange);

        foreach (var hit in hits)
        {
            hit.gameObject.TryGetComponent<GimmickBase>(out var gimmick);

            if (gimmick is not Mist mist)
                continue;

            mist.Winded();
        }

        CRISoundManager.Instance.PlaySE(SFX.CrowWing);
    }
}


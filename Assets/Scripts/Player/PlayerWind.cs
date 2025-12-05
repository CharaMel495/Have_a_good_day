using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWind
{
    public float windRange = 4f;
    public float windAngle = 45f;
    public float windPower = 20f;

    public ParticleSystem windEffect; // ‰H‚Î‚½‚«‚Ìƒp[ƒeƒBƒNƒ‹

    public Vector3 GetRightWindDir(Vector3 move)
    {
        // ã‰º¬•ª‚ğíœ‚µ‚Ä…•½‚¾‚¯‚É
        move.y = 0f;

        // “®‚«‚ª¬‚³‚·‚¬‚é‚Æ‚«‚Í–³•—
        if (move.sqrMagnitude < 0.0001f)
            return Vector3.zero;

        move.Normalize();

        // U‚è•ûŒü‚É‘Î‚µ g‰E‘¤h ‚Ì’¼Šp•ûŒü = Cross(up, move)
        Vector3 windDir = Vector3.Cross(Vector3.up, move).normalized;

        return windDir;
    }

    public Vector3 GetLeftWindDir(Vector3 move)
    {
        // ã‰º¬•ª‚ğíœ‚µ‚Ä…•½‚¾‚¯‚É
        move.y = 0f;

        // “®‚«‚ª¬‚³‚·‚¬‚é‚Æ‚«‚Í–³•—
        if (move.sqrMagnitude < 0.0001f)
            return Vector3.zero;

        move.Normalize();

        // U‚è•ûŒü‚É‘Î‚µ g¶‘¤h ‚Ì’¼Šp•ûŒü = Cross(move, up)
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
            Rigidbody rb = hit.attachedRigidbody;
            if (!rb) continue;

            rb.AddForce(windDir * windPower, ForceMode.Impulse);
        }

        CRISoundManager.Instance.PlaySE(SFX.CrowWing);
    }
}


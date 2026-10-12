using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GroundWalker : MonoBehaviour
{
    public Transform Target { get; set; }     // プレイヤー etc
    public float MoveSpeed { get; set; }

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Walk()
    {
        if (Target == null) 
            return;

        // 水平方向だけで追尾
        Vector3 dir = Target.position - transform.position;
        dir.y = 0f;
        dir = dir.normalized;

        Vector3 nextPos = _rb.position + dir * MoveSpeed * Time.fixedDeltaTime;

        _rb.MovePosition(nextPos);

        if (Vector3.Distance(nextPos, Target.position) < 1.0f)
        {
            Destroy(this.gameObject);
        }

        // 方向を向かせる
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(dir);

        
    }
}

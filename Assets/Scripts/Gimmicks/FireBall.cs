using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : GimmickBase
{
    public override void Initialize(Transform target)
    {
        var vec = target.position - this.transform.position;
        _rb.velocity = vec.normalized * _moveSpeed;
    }

    private void FixedUpdate()
    {

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    // 最初に当たったものの法線ベクトル方向に飛ぶ
    //    var vec = collision.contacts[0].normal.normalized;
    //    // Rigidbodyに瞬間的な力を加える
    //    _rb.AddForce(vec * _moveSpeed, ForceMode.Impulse);
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : GimmickBase
{
    public override void Initialize(Transform target)
    {
        var vec = target.position - this.transform.position;
        _rb.linearVelocity = vec.normalized * _moveSpeed;
    }

    private void FixedUpdate()
    {

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    // �ŏ��ɓ����������̖̂@���x�N�g�������ɔ��
    //    var vec = collision.contacts[0].normal.normalized;
    //    // Rigidbody�ɏu�ԓI�ȗ͂�������
    //    _rb.AddForce(vec * _moveSpeed, ForceMode.Impulse);
    //}
}

using CriWare;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : GimmickBase
{
    [SerializeField]
    private CriAtomSource _atomSource;

    private float _soundInterval = 0.4f;

    public override void Initialize(Transform target)
    {
        _timer = new();
        _timer.Initialize();

        _rb = this.GetComponent<Rigidbody>();
        var vec = target.position - this.transform.position;
        _rb.velocity = vec.normalized * _moveSpeed;

        Vector3 dir = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

        _timer.CreateTask(PlayWingSound, _soundInterval);
    }

    private void FixedUpdate()
    {
        _timer.Update();
    }

    private void PlayWingSound()
    {
        if (this.gameObject == null)
            return;

        CRISoundManager.Instance.PlaySE(SFX.CrowWing, _atomSource);
        _timer.CreateTask(PlayWingSound, _soundInterval);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    // 最初に当たったものの法線ベクトル方向に飛ぶ
    //    var vec = collision.contacts[0].normal.normalized;
    //    // Rigidbodyに瞬間的な力を加える
    //    _rb.AddForce(vec * _moveSpeed, ForceMode.Impulse);
    //}
}

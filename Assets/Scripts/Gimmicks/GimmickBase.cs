using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class GimmickBase : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem _deadEffect;
    [SerializeField]
    protected EffectController _effectController;
    [SerializeField]
    protected Vector3 _effectSize;
    public bool IsSheded { get; set; } = false;
    protected Rigidbody _rb;
    protected Timer _timer;
    [SerializeField]
    protected float _moveSpeed;
    public abstract void Initialize(Transform target);
    public virtual void Shed(Vector3 impactNormal)
    {
        IsSheded = true;
        // 最初に当たったものの法線ベクトル方向に飛ぶ
        var vec = -impactNormal;
        // Rigidbodyに瞬間的な力を加える
        _rb.AddForce(vec * 30, ForceMode.Impulse);

        _timer.CreateTask(() =>
        {
            if (_deadEffect != null)
            {
                var con = Instantiate(_effectController, this.transform.position, this.transform.rotation);
                con.Play(_deadEffect, 5.0f);
            }

            Destroy(this.gameObject);
        }, 1.0f);

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<GimmickBase>(out var gimmick))
        {
            gimmick.Shed(collision.contacts[0].normal.normalized);
        }
    }
}
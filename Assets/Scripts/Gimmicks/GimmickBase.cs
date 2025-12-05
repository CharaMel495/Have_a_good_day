using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class GimmickBase : MonoBehaviour
{
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
        _timer.CreateTask(() => Destroy(this.gameObject), 4.0f);
    }
}
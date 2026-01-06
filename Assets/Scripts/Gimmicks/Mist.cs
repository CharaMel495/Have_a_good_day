using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : GimmickBase
{
    [SerializeField]
    private float _mistValue;

    private float _currentValue;

    [SerializeField]
    private Material _mat;

    [SerializeField]
    private Color _mistColor;

    [SerializeField]
    private Color _flapedMistColor;


    public override void Initialize(Transform target)
    {
        var vec = target.position - this.transform.position;
        _rb = this.GetComponent<Rigidbody>();
        _rb.velocity = vec.normalized * _moveSpeed;
        _currentValue = _mistValue;

        var t = Mathf.InverseLerp(0, _mistValue, _currentValue);

        _mat.color = Color.Lerp(Color.clear, _mistColor, t);

        _timer = new();
    }

    public void Winded()
    {
        --_currentValue;

        var t = Mathf.InverseLerp(0, _mistValue, _currentValue);

        _mat.color = Color.Lerp(Color.clear, _mistColor, t);

        if (_currentValue > 0)
            return;

        Destroy(this.gameObject);
    }
}

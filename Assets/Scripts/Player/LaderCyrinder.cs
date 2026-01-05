using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaderCyrinder : MonoBehaviour
{
    public Transform PlayerTransform
    { get; set; }
    public Transform GimmickTransform
    { get; set; }

    [SerializeField]
    private float _distance;
    [SerializeField]
    private float _height;

    private void FixedUpdate()
    {
        if (GimmickTransform == null)
            Destroy(this.gameObject);

        SetPosition();
    }

    private void SetPosition()
    {
        var dir = GimmickTransform.position - PlayerTransform.position;

        var pos = PlayerTransform.position + (dir.normalized * _distance);
        pos.y = _height;

        this.transform.position = pos;

        this.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}

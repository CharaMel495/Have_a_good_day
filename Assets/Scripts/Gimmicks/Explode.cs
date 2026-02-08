using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    //private Timer _timer;

    //private void Start() => _timer = new();
    //private void FixedUpdate() => _timer.Update();

    //public void Begin(float explodeTime)
    //{
    //    _timer.CreateTask(() => Destroy(this.gameObject), explodeTime);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<GimmickBase>(out var gimmick))
        {
            //gimmick.Shed((other.transform.position - this.transform.position).normalized);
            gimmick.Shed((this.transform.position - other.transform.position).normalized);
        }
    }
}

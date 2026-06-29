using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    private Timer _timer;

    private Explode _explode;

    private void FixedUpdate()
    {
        _timer.Update();
    }

    public void Play(ParticleSystem particle, Explode explode, float lim)
    {
        _timer = new();
        var effect = Instantiate(particle, this.transform.position, Quaternion.identity);
        _explode = Instantiate(explode, this.transform.position, Quaternion.identity);
        _timer.CreateTask(End, lim);
    }

    private void End()
    {
        Destroy(this.gameObject);
        Destroy(_explode.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    private Timer _timer;

    private void FixedUpdate()
    {
        _timer.Update();
    }

    public void Play(ParticleSystem particle, float lim)
    {
        _timer = new();
        var effect = Instantiate(particle, this.transform);
        _timer.CreateTask(End, lim);
    }

    private void End()
    {
        Destroy(this.gameObject);
    }
}

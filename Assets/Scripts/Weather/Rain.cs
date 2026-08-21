using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour, IWeather
{
    [SerializeField]
    private float _keepTime;

    //[SerializeField]
    //private Debris _rainDebris;
    [SerializeField]
    private ParticleSystem _rainParticle;
    [SerializeField]
    private Color _debriColor;

    private Durator _durator;

    [Header("羽ばたきの影響受ける度合")]
    [SerializeField]
    private float _windowedRatio;

    private int _taskKey;

    public void Initialize()
    {
        _durator = new();
        this.gameObject.SetActive(false);
        _rainParticle.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        _durator.Update();
    }

    public void Open()
    {
        //_rainDebris.ChangeDebriColor(_debriColor);
        
        //this.gameObject.SetActive(true);
        _rainParticle.gameObject.SetActive(true);

        var em = _rainParticle.emission;
        em.rateOverTime = 1000;

        EventDispatcher.Instance.Dispatch("StartRain");
        _taskKey = _durator.CreateTask(UpdateDebriColor, () => EventDispatcher.Instance.Dispatch("EndWeather"), _keepTime);
    }

    public void Close()
    {
        //_rainDebris.ChangeDebriColor(Color.clear);

        EventDispatcher.Instance.Dispatch("EndRain");
        //this.gameObject.SetActive(false);
        _rainParticle.gameObject.SetActive(false);
    }

    public void UpdateDebriColor(float elapsedTime, float endTime)
    {
        var t = Mathf.InverseLerp(0.0f, endTime, elapsedTime);
        var col = Mathf.Lerp(0, 1000, t);
        var em = _rainParticle.emission;
        em.rateOverTime = col;
    }

    public void ApplyWindow()
    {
        _durator.ForceAdvanceTask(_taskKey, _windowedRatio);
    }
}

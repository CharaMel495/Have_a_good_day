using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : MonoBehaviour, IWeather
{
    [SerializeField]
    private float _keepTime;

    [SerializeField]
    private float _initializeMistTime;

    private Durator _durator;

    [SerializeField]
    private Material _mat;

    [SerializeField]
    private float _maxMist;

    [Header("羽ばたきの影響受ける度合")]
    [SerializeField]
    private float _windowedRatio;

    private int _taskKey;

    public void Initialize()
    {
        _durator = new();

        _mat.SetFloat("_FogDensity", 0.0f);

        this.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        _durator.Update();
    }

    public void Open()
    {
        this.gameObject.SetActive(true);

        _durator.CreateTask(InMist, () => _taskKey = _durator.CreateTask(OutMist, () => EventDispatcher.Instance.Dispatch("EndWeather"), _keepTime), _initializeMistTime);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void InMist(float elapsedTime, float endTime)
    {
        var t = Mathf.InverseLerp(0.0f, endTime, elapsedTime);
        _mat.SetFloat("_FogDensity", Mathf.Lerp(0.0f, _maxMist, t));
    }

    public void OutMist(float elapsedTime, float endTime)
    {
        var t = Mathf.InverseLerp(0.0f, endTime, elapsedTime);
        _mat.SetFloat("_FogDensity", Mathf.Lerp(_maxMist, 0.0f, t));
    }

    public void ApplyWindow()
    {
        _durator.ForceAdvanceTask(_taskKey, _windowedRatio);
    }
}

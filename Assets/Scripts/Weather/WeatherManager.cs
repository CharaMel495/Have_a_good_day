using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeather
{
    public void Open();
    public void Close();
}

public class WeatherManager : SingletonMonoBehaviour<WeatherManager>
{
    public enum eWeater
    {
        晴れ,
        雨,
        霧
    }

    [SerializeField]
    private Rain _rain;

    [SerializeField]
    private Mist _mist;

    private Dictionary<eWeater, IWeather> _weathers;

    private bool _isWeather = false;

    // Start is called before the first frame update
    public void Initialize()
    {
        _rain.Initialize();
        _mist.Initialize();

        _weathers = new Dictionary<eWeater, IWeather>
        {
            { eWeater.晴れ, null },
            { eWeater.雨, _rain },
            { eWeater.霧, _mist }
        };

        EventDispatcher.Instance.Bind(this);

        _isWeather = false;
    }

    [CallableEvent("OpenWeather")]
    public void OpenWeather(object data)
    {
        if (_isWeather)
            return;

        if (data is eWeater weather)
        {
            _weathers[weather].Open();
            _isWeather = true;
        }
    }

    [CallableEvent("EndWeather")]
    public void EndWeather(object data)
    {
        if (!_isWeather)
            return;

        if (data is eWeater weather)
        {
            _weathers[weather].Close();
            _isWeather = false;
        }
    }

    [CallableEvent("ApplyWind")]
    public void Windowed(object data)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeatherManager;

public interface IWeather
{
    public void Open();
    public void Close();
    public void ApplyWindow();
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

    private IWeather _currentWeather = null;

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
        _currentWeather = null;
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
            _currentWeather = _weathers[weather];
        }
    }

    [CallableEvent("EndWeather")]
    public void EndWeather(object data)
    {
        if (!_isWeather)
            return;

        _currentWeather.Close();
        _isWeather = false;
        _currentWeather = null;
    }

    [CallableEvent("ApplyWind")]
    public void Windowed(object data)
    {
        if (_currentWeather != null)
            _currentWeather.ApplyWindow();
    }
}

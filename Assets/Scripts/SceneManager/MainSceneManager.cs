using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// メインシーンについて管理するクラス
/// </summary>
public class MainSceneManager : SceneManagerBase<MainSceneManager>
{
    [SerializeField]
    private ScoreDataAsset _scoreAsset;

    [SerializeField]
    private Material _skyMat;

    [SerializeField]
    [Header("デバッグ用オブジェクト")]
    private GameObject _debugCanvus;

    [Header("ステージのタイムライン")]
    [SerializeField]
    private float _stageTime;
    [SerializeField]
    private float _rainTime;
    [SerializeField]
    private float _mistTime;

    [SerializeField]
    private SunRotater _rotator;

    private float _remainTime;

    private bool _isRained;
    private bool _isMisted;

    public override void Initialize()
    {
        InputSystemManager.BindAction("OpenUI", SwitchDebugCanvusActive);

        PlayerManager.Instance.Initialize();

        CRISoundManager.Instance.PlayBGM(BGM.Test2);

        WeatherManager.Instance.Initialize();

        _remainTime = _stageTime;
        _isRained = false;
        _isMisted = false;

        _scoreAsset.WasSurvived = false;
    }

    private void FixedUpdate()
    {
        _remainTime -= Time.fixedDeltaTime;

        var t = Mathf.InverseLerp(_stageTime, 0.0f, _remainTime);
        _skyMat.SetFloat("_SkyTime", t);

        _rotator.RotateSun(t);

        if (!_isRained && _remainTime < _rainTime)
        {
            _isRained = true;
            EventDispatcher.Instance.Dispatch("OpenWeather", WeatherManager.eWeater.雨);
        }

        if (!_isMisted && _remainTime < _mistTime)
        {
            _isMisted = true;
            EventDispatcher.Instance.Dispatch("OpenWeather", WeatherManager.eWeater.霧);
        }

        if (_remainTime > 0)
            return;

        _scoreAsset.WasSurvived = true;
        GameManager.ToGameOverScene();
    }

    public void SwitchDebugCanvusActive()
    {
        _debugCanvus.gameObject.SetActive(!_debugCanvus.gameObject.activeSelf);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
    [SerializeField]
    private GameObject _doorR;
    [SerializeField]
    private GameObject _doorL;
    [SerializeField]
    private float _pow;
    [SerializeField]
    private Mist _mist;

    private bool _isStarted;

    private Timer _timer;

    public void Initialize()
    {
        _isStarted = false;

        _mist.Initialize();
        InputSystemManager.BindAction("DebugJump", StartGame);
        CRISoundManager.Instance.PlayBGM(BGM.Title_BGM);

        EventDispatcher.Instance.Subscribe("StartGame", (_) => StartGame());
        //EventDispatcher.Instance.Subscribe("OnWingFlapped", (_) => StartGame());

        _timer = new();
        _timer.Initialize();
    }

    private void FixedUpdate()
    {
        _timer.Update();
    }

    public void StartGame()
    {
        if (_isStarted)
            return;

        _isStarted = true;

        //var rbR = _doorR.AddComponent<Rigidbody>();
        //var rbL = _doorL.AddComponent<Rigidbody>();

        //rbR.AddForce(Vector3.forward * _pow, ForceMode.Impulse);
        //rbL.AddForce(Vector3.forward * _pow, ForceMode.Impulse);

        CRISoundManager.Instance.PlaySE(SFX.OpenTheDoor);

        _mist.Open();

        _timer.CreateTask(() => GameManager.ToMainScene(), 5.0f);
    }
}

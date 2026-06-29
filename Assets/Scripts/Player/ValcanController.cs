using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValcanController : MonoBehaviour
{
    [SerializeField]
    private Valcan _valcan;

    [SerializeField]
    private float _valcanAppearTime;

    private const float _CLOSEDEULERZ = -180.0f;
    private const float _OPENEDEULERZ = 0.0f;

    private Timer _timer;
    private Durator _durator;
    private const float _INITIALIZEDELAY = 1.0f;

    private bool _isAwaken = false;

    // Start is called before the first frame update
    void Start()
    {
        _timer = new();
        _durator = new();
        _timer.CreateTask(Initialize, _INITIALIZEDELAY);
    }

    private void FixedUpdate()
    {
        _timer.Update();
        _durator.Update();
    }

    private void Initialize()
    {
        _isAwaken = false;

        this.gameObject.SetActive(false);
        EventDispatcher.Instance.Subscribe("StartValcan", (object temp) => Appear());
        EventDispatcher.Instance.Subscribe("EndValcan", (object temp) => Disappear());
    }

    public void Appear()
    {
        if (_isAwaken)
            return;

        _isAwaken = true;

        this.gameObject.SetActive(true);
        //_durator.CreateTask(Open, _valcan.Begin, _valcanAppearTime);
        Open();
    }

    public void Open()
    {
        this.transform.DOLocalMoveX(-2, _valcanAppearTime).OnComplete(_valcan.Begin);
    }


    public void Open(float elapsedTime, float endTime)
    {
        var euler = this.transform.eulerAngles;
        
        var t = Mathf.InverseLerp(endTime, 0.0f, elapsedTime);
        euler.z = Mathf.Lerp(_OPENEDEULERZ, _CLOSEDEULERZ, t);

        Debug.Log(euler.z);

        this.transform.eulerAngles = euler;
    }

    public void Close()
    {
        this.transform.DOLocalMoveX(-12, _valcanAppearTime).OnComplete(End);
    }

    public void Close(float elapsedTime, float endTime)
    {
        var euler = this.transform.eulerAngles;

        var t = Mathf.InverseLerp(0, endTime, elapsedTime);
        euler.z = Mathf.Lerp(_OPENEDEULERZ, _CLOSEDEULERZ, t);

        this.transform.eulerAngles = euler;
    }

    public void Disappear()
    {
        //_durator.CreateTask(Close, End, _valcanAppearTime);
        Close();
    }

    private void End()
    {
        _isAwaken = false;
        this.gameObject.SetActive(false);
    }
}

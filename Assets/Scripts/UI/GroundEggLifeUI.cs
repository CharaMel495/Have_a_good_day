using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEggLifeUI : MonoBehaviour
{
    [SerializeField]
    private TextWrapper[] _texts;
    [SerializeField]
    private Color _textColor = Color.white;
    [SerializeField]
    private float _fadeTime;

    private const float _INITIALIZEDELAY = 1.0f;

    private Timer _timer;
    private Durator _durator;

    private int _taskID;

    // Start is called before the first frame update
    void Start()
    {
        _timer = new();
        _durator = new();
        foreach (var text in _texts)
        {
            text.Initialize();
            text.SetTextColor(_textColor);
            text.SetTextAlpha(0.0f);
        }
        _timer.CreateTask(() => EventDispatcher.Instance.Bind(this), _INITIALIZEDELAY);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _timer.Update();
        _durator.Update();
    }

    [CallableEvent("OnLifeChanged")]
    public void UpdateLifeValue(object data)
    {
        if (data is not int life)
            return;

        _texts[0].SetText(life.ToString());
        foreach (var text in _texts)
        {
            text.SetTextAlpha(1.0f);
        }

        _durator.CanncellTask(_taskID);

        _taskID = _durator.CreateTask(FadeOutText, () =>
        {
            foreach (var text in _texts)
                text.SetTextAlpha(0.0f);
        }, _fadeTime);
    }

    public void FadeOutText(float elapsedTime, float endTime)
    {
        foreach (var text in _texts)
            text.SetTextAlpha(Mathf.InverseLerp(endTime, 0, elapsedTime));
    }
}

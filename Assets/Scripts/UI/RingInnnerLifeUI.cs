using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingInnnerLifeUI : MonoBehaviour
{
    [SerializeField]
    private TextWrapper _text;
    [SerializeField]
    private Color _textColor = Color.white;
    

    private const float _INITIALIZEDELAY = 1.0f;

    private Timer _timer;

    // Start is called before the first frame update
    void Start()
    {
        _timer = new();
        _text.Initialize();
        _text.SetTextColor(_textColor);
        _timer.CreateTask(() => EventDispatcher.Instance.Bind(this), _INITIALIZEDELAY);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _timer.Update();
    }

    [CallableEvent("OnLifeChanged")]
    public void UpdateLifeValue(object data)
    {
        if (data is not int life)
            return;

        _text.SetText(life.ToString());
    }
}

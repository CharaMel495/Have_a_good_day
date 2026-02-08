using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBird : MonoBehaviour
{
    [SerializeField]
    private TextWrapper _text;
    [SerializeField]
    private Color _textColor = Color.white;

    [Header("Angle (Degree)")]
    [SerializeField]
    private float _startAngleDeg = -60f;
    [SerializeField]
    private float _endAngleDeg = 60f;

    [SerializeField]
    private Transform _player;
    [SerializeField]
    private float _radius = 1.5f;
    [SerializeField]
    private float _height = 3.0f;
    [SerializeField]
    private float _duration = 2.0f;

    //float timer = 0f;

    private const float _INITIALIZEDELAY = 1.0f;

    private Timer _timer;
    private Durator _durator;

    private bool _isMoving;

    // Start is called before the first frame update
    void Start()
    {
        _timer = new();
        _durator = new();
        _text.Initialize();
        _text.SetTextColor(_textColor);
        _timer.CreateTask(() => { EventDispatcher.Instance.Bind(this); this.gameObject.SetActive(false); }, _INITIALIZEDELAY);

        _isMoving = false;

        Move(0, _duration);

        
    }

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

        _text.SetText(life.ToString());
        this.gameObject.SetActive(true);

        if (!_isMoving)
        {
            _isMoving = true;
            _durator.CreateTask(Move, () => _isMoving = false, _duration);
        }
    }

    public void Move(float elapsedTime, float endTime)
    {
        float t = Mathf.Clamp01(elapsedTime / endTime);

        float angleRad = Mathf.Lerp(
            _startAngleDeg * Mathf.Deg2Rad,
            _endAngleDeg * Mathf.Deg2Rad,
            t
        );

        // === 公転位置 ===
        Vector3 center = _player.position;
        Vector3 offset = new Vector3(
            Mathf.Cos(angleRad) * _radius,
            _height,
            Mathf.Sin(angleRad) * _radius
        );

        transform.position = center + offset;

        // === 向き：ローカル後ろ(-forward)をプレイヤーへ ===
        Vector3 toPlayer = center - transform.position;

        // forward を反転させたいので LookRotation に -toPlayer
        transform.rotation = Quaternion.LookRotation(-toPlayer, Vector3.up);
    }
}

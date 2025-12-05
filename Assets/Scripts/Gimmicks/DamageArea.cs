using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField]
    private int _maxLife = 10;
    private int _currentLife;

    [SerializeField]
    private Color _startColor;

    [SerializeField]
    private Color _endColor;

    [SerializeField]
    private Material _material;

    [SerializeField]
    private float _suviveTime;
    private float _currentSurviveTime;

    [SerializeField]
    private ScoreDataAsset _scoreAsset;

    [SerializeField]
    private TextWrapper _text;

    // Start is called before the first frame update
    void Start()
    {
        _material.color = _startColor;
        _currentLife = _maxLife;
        _currentSurviveTime = 0f;
        _scoreAsset.SuviveTime = 0f;
        _scoreAsset.WasSurvived = false;
        //_text.Initialize();
        //_text.SetText($"{(int)_currentSurviveTime}");
    }

    private void FixedUpdate()
    {
        //_currentSurviveTime += Time.fixedDeltaTime;
        //_scoreAsset.SuviveTime = _currentSurviveTime;
        ////_text.SetText($"{(int)(_suviveTime - _currentSurviveTime)}");

        //if (_currentSurviveTime > _suviveTime)
        //{
        //    _scoreAsset.WasSurvived = true;
        //    GameManager.ToGameOverScene();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<FireBall>(out var fireball))
        {
            --_currentLife;
            float t = Mathf.InverseLerp(_maxLife, 0, _currentLife);
            _material.color = Color.Lerp(_startColor, _endColor, t);

            if (_currentLife < 0)
                GameManager.ToGameOverScene();
        }
    }

    //private void OnTriggerStay(Tri collision)
    //{
    //    if (collision.gameObject.TryGetComponent<FireBall>(out var fireball))
    //    {
    //        float t = Mathf.InverseLerp(_life, 0, _currentLife);
    //        _material.color = Color.Lerp(_startColor, _endColor, t);
    //    }
    //}
}

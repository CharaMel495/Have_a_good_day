using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField]
    private EggPlant _eggPlant;

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
        if (other.gameObject.TryGetComponent<GimmickBase>(out var gimmick))
        {
            _eggPlant.Damage();
            Destroy(gimmick.gameObject);
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

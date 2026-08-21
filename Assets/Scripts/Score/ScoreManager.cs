using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    [SerializeField]
    private ScoreDataAsset _scoreData;
    [SerializeField]
    private float _comboKeepTime;
    private int _currentCombo;
    private int _maxCombo;
    private float _remainComboKeepTime;

    // Start is called before the first frame update
    public void Initialize()
    {
        _currentCombo = 0;
        _maxCombo = 0;

        _scoreData.ComboCount = 0;
        _scoreData.EggValue = 10;
        _scoreData.WingFrappCount = 0;

        EventDispatcher.Instance.Bind(this);
    }

    private void FixedUpdate()
    {
        if (_currentCombo > 0)
            JudgeComboKeep();
    }

    private void JudgeComboKeep()
    {
        _remainComboKeepTime -= Time.fixedDeltaTime;

        if (_remainComboKeepTime > 0)
            return;

        _currentCombo = 0;
    }

    [CallableEvent("OnGimmickEliminated")]
    public void OnGimmickEliminated(object _)
    {
        ++_currentCombo;
        _remainComboKeepTime = _comboKeepTime;

        if (_currentCombo > _maxCombo)
            _maxCombo = _currentCombo;
    }

    [CallableEvent("OnWingFlapped")]
    public void OnWingFlapped()
    {
        ++_scoreData.WingFrappCount;
    }
}

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

    [Header("UI系")]
    [SerializeField]
    private TextWrapper[] _textUIs;
    [SerializeField]
    private ImageWrapper _scoreImage;
    [SerializeField]
    private Sprite[] _scoreSprites;

    // Start is called before the first frame update
    public void Initialize()
    {
        _currentCombo = 0;
        _maxCombo = 0;

        _scoreImage.Initialize();

        foreach (var ui in _textUIs)
            ui.Initialize();

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
    public void OnWingFlapped(object _)
    {
        ++_scoreData.WingFrappCount;
    }

    [CallableEvent("OnViewScore")]
    public void OnViewScore(object _)
    {
        _textUIs[0].SetText($"{_scoreData.EggValue}コ");
        _textUIs[1].SetText($"{_maxCombo}コンボ");
        _textUIs[2].SetText($"{_scoreData.WingFrappCount}回");

        _scoreImage.SetSprite(
            _scoreData.EggValue switch
            {
                10 => _scoreSprites[0],
                9 => _scoreSprites[1],
                8 => _scoreSprites[1],
                7 => _scoreSprites[1],
                6 => _scoreSprites[2],
                5 => _scoreSprites[2],
                4 => _scoreSprites[2],
                3 => _scoreSprites[3],
                2 => _scoreSprites[3],
                1 => _scoreSprites[3],
                _ => _scoreSprites[4]
            });
    }
}

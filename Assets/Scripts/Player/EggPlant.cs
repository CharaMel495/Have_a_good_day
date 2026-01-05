using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggPlant : MonoBehaviour
{
    [SerializeField]
    private float _needWarmingTime;
    [SerializeField]
    private float _warmDamage;

    [SerializeField]
    private float _warmSpeed;
    [SerializeField]
    private float _warmSpeed_Crouch;

    [SerializeField]
    private SelfMade.Slider _slider;

    [SerializeField]
    private ScoreDataAsset _scoreAsset;

    [SerializeField]
    private SunRotater _sun;

    private float _warmingTime;

    private float _changeA;
    private float _changeB;

    private bool _hasChangedA;
    private bool _hasChangedB;

    public void Initialize()
    {
        _warmingTime = 0;
        float t = Mathf.InverseLerp(0, _needWarmingTime, _warmingTime);
        _slider.UpdateValue(t);
        _changeA = 10.0f;
        _changeB = 20.0f;

        CRISoundManager.Instance.SetCurrentBGMAISAC("AisacControl_00", 0.0f);
        CRISoundManager.Instance.SetCurrentBGMAISAC("AisacControl_01", 1.0f);
        CRISoundManager.Instance.SetCurrentBGMAISAC("AisacControl_02", 0.0f);

        _hasChangedA = false;
        _hasChangedB = false;
    }

    public void Warm(bool isStand)
    {
        _warmingTime += Time.fixedDeltaTime * (isStand ? _warmSpeed : _warmSpeed_Crouch);

        float t = Mathf.InverseLerp(0, _needWarmingTime, _warmingTime);
        _slider.UpdateValue(t);
        _sun.RotateSun(t);

        // 徐々に音を足すテストコード
        if (_warmingTime > _changeA && !_hasChangedA)
        {
            CRISoundManager.Instance.SetCurrentBGMAISAC("AisacControl_02", 1.0f);
            _hasChangedA = true;
        }

        if (_warmingTime > _changeB && !_hasChangedB)
        {
            CRISoundManager.Instance.SetCurrentBGMAISAC("AisacControl_00", 1.0f);
            _hasChangedB = true;
        }

        // ここにゲームクリアを書く

        if (_warmingTime > _needWarmingTime)
        {
            _scoreAsset.WasSurvived = true;
            GameManager.ToGameOverScene();
        }
    }

    public void Damage()
    {
        _warmingTime -= _warmDamage;

        float t = Mathf.InverseLerp(0, _needWarmingTime, _warmingTime);
        _slider.UpdateValue(t);

        CRISoundManager.Instance.PlaySE(SFX.EggBreak);

        if (_warmingTime < 0)
            GameManager.ToGameOverScene();
    }
}

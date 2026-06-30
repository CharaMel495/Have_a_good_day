using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    [SerializeField]
    private ScoreDataAsset _scoreData;
    [SerializeField]
    private int _comboKeepTime;
    private int _currentCombo;
    private int _maxCombo;

    // Start is called before the first frame update
    public void Initialize()
    {
           
    }

    [CallableEvent("OnGimmickEliminated")]
    public void OnGimmickEliminated(object _)
    {

    }

    [CallableEvent("OnWingFlapped")]
    public void OnWingFlapped()
    {

    }
}

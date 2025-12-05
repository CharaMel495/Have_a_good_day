using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverTextChanger : MonoBehaviour
{
    [SerializeField]
    private ScoreDataAsset _scoreDataAsset;

    [SerializeField]
    private TextMeshPro _text;

    private void Start()
    {
        _text.text = (_scoreDataAsset.WasSurvived ? "GameClear!" : "GameOver");
    }
}

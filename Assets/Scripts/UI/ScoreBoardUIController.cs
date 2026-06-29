using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardUIController : MonoBehaviour
{
    [SerializeField]
    private TextWrapper _textUI;

    [SerializeField]
    private ScoreBoardDataAsset _scoreDataAsset;

    // Start is called before the first frame update
    void Start()
    {
        _textUI.Initialize();
        ShowScore();   
    }

    private void ShowScore()
    {
        _textUI.ClearText();

        for (int i = 0; i < 5; ++i)
        {
            _textUI.AddText(_scoreDataAsset.GetScoreText(i));
            _textUI.AddText('\n');
        }
    }
}

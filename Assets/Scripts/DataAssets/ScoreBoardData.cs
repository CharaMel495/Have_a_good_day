using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScoreBoardDataAsset", fileName = "ScoreBoardDataAsset")]
public class ScoreBoardDataAsset : ScriptableObject
{
    public int[] Scores;

    public string GetScoreText(int ranking)
    {
        return ranking switch
        {
            0 => $"1st:{Scores[0]:'0000'}pts",
            1 => $"2nd:{Scores[1]:'0000'}pts",
            2 => $"3rd:{Scores[2]:'0000'}pts",
            3 => $"4th:{Scores[3]:'0000'}pts",
            4 => $"5th:{Scores[4]:'0000'}pts",
            _ => ""
        };
    }
}


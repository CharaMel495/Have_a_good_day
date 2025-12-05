using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScoreDataAsset", fileName = "ScoreDataAsset")]
public class ScoreDataAsset : ScriptableObject
{
    public float SuviveTime;
    public bool WasSurvived;
}

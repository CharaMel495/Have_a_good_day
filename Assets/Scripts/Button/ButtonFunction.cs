using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunction : MonoBehaviour
{
    public void LoadMainScene()
    {
        GameManager.ToMainScene();
    }

    public void LoadGameOver()
    {
        GameManager.ToGameOverScene();
    }

    public void LoadTitle()
    {
        GameManager.ToTitileScene();
    }

    public void EndGame()
    {
        GameManager.EndGame();
    }
}

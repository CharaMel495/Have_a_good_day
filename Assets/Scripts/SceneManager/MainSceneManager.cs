using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// メインシーンについて管理するクラス
/// </summary>
public class MainSceneManager : SceneManagerBase<MainSceneManager>
{
    [SerializeField]
    [Header("デバッグ用オブジェクト")]
    private GameObject _debugCanvus;

    public override void Initialize()
    {
        InputSystemManager.BindAction("OpenUI", SwitchDebugCanvusActive);

        PlayerManager.Instance.Initialize();

        CRISoundManager.Instance.PlayBGM(BGM.Test2);
    }

    public void SwitchDebugCanvusActive()
    {
        _debugCanvus.gameObject.SetActive(!_debugCanvus.gameObject.activeSelf);
    }
}

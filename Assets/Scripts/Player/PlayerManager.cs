using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    [SerializeField]
    private Player _player;

    public void Initialize()
    {
        _player.Initialize();
    }


}

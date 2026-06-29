using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
    [SerializeField]
    private GameObject _doorR;
    [SerializeField]
    private GameObject _doorL;
    [SerializeField]
    private float _pow;
    [SerializeField]
    private Mist _mist;

    public void Initialize()
    {
        _mist.Initialize();
        InputSystemManager.BindAction("DebugJump", StartGame);
    }

    public void StartGame()
    {
        var rbR = _doorR.AddComponent<Rigidbody>();
        var rbL = _doorL.AddComponent<Rigidbody>();

        rbR.AddForce(Vector3.forward * _pow, ForceMode.Impulse);
        rbL.AddForce(Vector3.forward * _pow, ForceMode.Impulse);

        _mist.Open();
    }
}

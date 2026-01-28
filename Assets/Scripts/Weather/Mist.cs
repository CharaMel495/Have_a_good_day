using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : MonoBehaviour, IWeather
{
    [SerializeField]
    private float _keepTime;

    private Durator _durator;

    public void Initialize()
    {
        _durator = new();
    }

    private void FixedUpdate()
    {
        _durator.Update();
    }

    public void Open()
    {

    }

    public void Close()
    {

    }
}

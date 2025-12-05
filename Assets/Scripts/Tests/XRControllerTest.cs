using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRControllerTest : MonoBehaviour
{
    void Start()
    {
        //InputSystemManager.Instance.Initialize();
    }

    void Update()
    {
        bool triggerValue = InputSystemManager.IsTriggerPressed(true);
        if (triggerValue)
        {
            Debug.Log("Trigger pressed: " + triggerValue);
        }
    }
}

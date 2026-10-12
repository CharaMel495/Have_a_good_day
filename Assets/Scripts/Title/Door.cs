using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        EventDispatcher.Instance.Dispatch("StartGame");
        this.GetComponent<Rigidbody>().AddForce(Vector3.forward * 50.0f, ForceMode.Impulse);
    }
}

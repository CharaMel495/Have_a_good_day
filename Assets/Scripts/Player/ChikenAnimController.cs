using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChikenAnimController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private int _crouchFlagHash = Animator.StringToHash("IsCrouch");

    private Timer _timer; 

    // Start is called before the first frame update
    void Start()
    {
        _timer = new();
        _timer.Initialize();

        _timer.CreateTask(SwitchIdle, 2.0f);
    }

    private void FixedUpdate()
    {
        _timer.Update();
    }

    private void SwitchIdle()
    {
        _animator.SetBool(_crouchFlagHash, !_animator.GetBool(_crouchFlagHash));

        _timer.CreateTask(SwitchIdle, 2.0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashAnimationTrigger : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void incrementInteger(string name)
    {
        _animator.SetInteger(name, _animator.GetInteger(name) + 1);
    }
}

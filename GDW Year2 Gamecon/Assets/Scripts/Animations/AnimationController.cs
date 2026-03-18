using System;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    
    private bool _isTwerking = false;

    public void SetAnimation(string name, bool active)
    {
        _animator.SetBool(name, active);
    }
    
    
    

    public void StopAnimation()
    {
        _animator.SetBool("parry", false);
        _animator.SetBool("gotHit", false);
        _animator.SetBool("shooting", false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            print("Twerking");
            _isTwerking = !_isTwerking;
            _animator.SetBool("Emote6", _isTwerking);
        }
    }
}

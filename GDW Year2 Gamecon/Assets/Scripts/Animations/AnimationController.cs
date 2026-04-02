using System;
using Unity.Netcode;
using UnityEngine;

public class AnimationController : NetworkBehaviour
{

    [SerializeField] private Animator _animator;
    
    private bool _isTwerking = false;

    public void SetAnimation(string name, bool active)
    {
        //StopEmotes();
        _animator.SetBool(name, active);
    }
    
    
    

    public void StopAnimation()
    {
        _animator.SetBool("parry", false);
        _animator.SetBool("gotHit", false);
        _animator.SetBool("shooting", false);
    }

    private void StartAnimate(string anima)
    {
        _isTwerking = !_isTwerking;
        
        if (_isTwerking)
        {
            _animator.SetBool(anima, true);
        }
        else
        {
            StopEmotes();
        }
    }

    public void StopEmotes()
    {
        if (!IsOwner) return;
        _isTwerking = false;
        _animator.SetBool("Emote1", false);
        _animator.SetBool("Emote2", false);
        _animator.SetBool("Emote3", false);
        _animator.SetBool("Emote4", false);
        _animator.SetBool("Emote5", false);
        _animator.SetBool("Emote6", false);
    }

    private void Update()
    {
        if (!IsOwner) return;
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartAnimate("Emote6");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartAnimate("Emote5");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartAnimate("Emote4");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartAnimate("Emote3");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartAnimate("Emote2");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartAnimate("Emote1");
        }
    }
}

// print("Twerking");
// _isTwerking = !_isTwerking;
//_animator.SetBool("Emote6", _isTwerking);

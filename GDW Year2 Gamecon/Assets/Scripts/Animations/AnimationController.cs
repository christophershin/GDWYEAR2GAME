using System;
using Unity.Netcode;
using UnityEngine;

public class AnimationController : NetworkBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shoot, parry, hit;
    
    private string _lastEmote = "null";

    public void SetAnimation(string nam, bool active)
    {


        if (nam == "gotHit")
        {
            MakeSoundClientRpc(nam);
        }
        else
        {
            MakeSoundServerRpc(nam);
        }
        
        
        //StopEmotes();
        _animator.SetBool(nam, active);
        
    }

    public void StopAnimation()
    {
        _animator.SetBool("parry", false);
        _animator.SetBool("gotHit", false);
        _animator.SetBool("shooting", false);
    }

    [ServerRpc]
    private void MakeSoundServerRpc(string sound)
    {
        MakeSoundClientRpc(sound);
    }
    
    [ClientRpc]
    private void MakeSoundClientRpc(string sound)
    {
        if (sound == "shooting")
        {
            audioSource.clip = shoot;
            audioSource.Play();
        }
        else if (sound == "parry")
        {
            audioSource.clip = parry;
            audioSource.Play();
        }
        else if (sound == "gotHit")
        {
            audioSource.clip = hit;
            audioSource.Play();
        }
    }

    private void StartAnimate(string anima)
    {
        if (_lastEmote != anima)
        {
            StopEmotes();
            _lastEmote = anima;
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
        _animator.SetBool("Emote1", false);
        _animator.SetBool("Emote2", false);
        _animator.SetBool("Emote3", false);
        _animator.SetBool("Emote4", false);
        _animator.SetBool("Emote5", false);
        _animator.SetBool("Emote6", false);
        _lastEmote = "null";
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

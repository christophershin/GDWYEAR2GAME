using UnityEngine;

public class AnimationController : MonoBehaviour
{

    [SerializeField] private Animator _animator;

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
}

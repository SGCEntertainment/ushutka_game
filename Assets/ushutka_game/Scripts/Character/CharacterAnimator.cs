using UnityEngine;

public class CharacterAnimator : CharacterComponent
{
    Animator _animator;

    int idleID = Animator.StringToHash("IsIdle");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetSpeed(bool isIdle)
    {
        _animator.SetBool(idleID, isIdle);
    }
}

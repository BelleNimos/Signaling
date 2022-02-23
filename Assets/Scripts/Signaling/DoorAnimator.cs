using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorAnimator : MonoBehaviour
{
    private Animator _animator;

    private const string Open = "Open";

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    public void EnableAnimation()
    {
        _animator.SetBool(Open, true);
    }

    public void DisableAnimation()
    {
        _animator.SetBool(Open, false);
    }
}

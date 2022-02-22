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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
            _animator.SetBool(Open, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _animator.SetBool(Open, false);
    }
}
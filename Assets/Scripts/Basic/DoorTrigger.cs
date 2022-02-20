using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorTrigger : MonoBehaviour
{
    private Animator _animator;

    private const string _open = "Open";

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PhysicsMovement>(out PhysicsMovement physicsMovement))
        {
            _animator.SetBool(_open, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _animator.SetBool(_open, false);
    }
}

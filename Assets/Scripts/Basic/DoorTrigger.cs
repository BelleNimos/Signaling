using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorTrigger : MonoBehaviour
{
    private Animator _animator;

    private const string Open = "Open";

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PhysicsMovement>(out PhysicsMovement physicsMovement))
            _animator.SetBool(Open, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _animator.SetBool(Open, false);
    }
}

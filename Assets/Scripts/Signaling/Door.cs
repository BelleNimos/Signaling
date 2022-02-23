using UnityEngine;

[RequireComponent(typeof(DoorAnimator))]
public class Door : MonoBehaviour
{
    private DoorAnimator _doorAnimator;

    private void OnEnable()
    {
        _doorAnimator = GetComponent<DoorAnimator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
            _doorAnimator.EnableAnimation();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _doorAnimator.DisableAnimation();
    }
}

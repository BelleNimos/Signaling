using UnityEngine;

public class TurnDirection : MonoBehaviour
{
    private Vector2 _targetVelocity;
    private bool _isFacingRight = true;

    private void Update()
    {
        if (_targetVelocity.x < 0 && _isFacingRight)
            Turn();
        else if (_targetVelocity.x > 0 && _isFacingRight == false)
            Turn();
    }

    private void Turn()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

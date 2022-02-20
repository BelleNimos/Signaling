using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private readonly float _speed;
    [SerializeField] private readonly float _jumpForce;
    [SerializeField] private readonly float _groundOffsetY;
    [SerializeField] private readonly float _groundRadius;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private float _horizontalMove = 0f;
    private bool _facingRight = true;
    private bool _isGrounded = false;

    private const string _jump = "Jump";
    private const string _walk = "Walk";

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * _speed;

        if (_horizontalMove < 0 && _facingRight)
            Turn();
        else if (_horizontalMove > 0 && !_facingRight)
            Turn();

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _animator.SetTrigger(_jump);
            _rigidbody2D.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        float speed = 1f;
        Vector2 targetVelocity = new Vector2(_horizontalMove * speed, _rigidbody2D.velocity.y);
        _animator.SetBool(_walk, Mathf.Abs(_horizontalMove) >= 0.1f);
        _rigidbody2D.velocity = targetVelocity;
        CheckGround();
    }

    private void Turn()
    {
        _facingRight = !_facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void CheckGround()
    {
        Collider2D[] coliders = Physics2D.OverlapCircleAll
            (new Vector2(transform.position.x, transform.position.y + _groundOffsetY), _groundRadius);

        if (coliders.Length > 1)
            _isGrounded = true;
        else
            _isGrounded = false;
    }
}

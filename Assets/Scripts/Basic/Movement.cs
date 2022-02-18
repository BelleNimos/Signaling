using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundOffsetY;
    [SerializeField] private float _groundRadius;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private float _horizontalMove = 0f;
    private bool _facingRight = true;
    private bool _isGrounded = false;

    private void Start()
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
            _animator.SetTrigger("Jump)");
            _rigidbody2D.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        float speed = 1f;
        Vector2 targetVelocity = new Vector2(_horizontalMove * speed, _rigidbody2D.velocity.y);
        _animator.SetBool("Walk", Mathf.Abs(_horizontalMove) >= 0.1f);
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
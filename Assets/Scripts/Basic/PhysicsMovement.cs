using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsMovement : MonoBehaviour
{
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private LayerMask _layerMask;

    private Animator _animator;
    private bool _isFacingRight = true;

    private const string _playerJump = "Jump";
    private const string _playerWalk = "Walk";
    private const string _playerRun = "Run";
    private const float _minGroundNormalY = 0.65f;
    private const float _gravityModifier = 1f;
    private const float _speed = 3f;
    private const float _speedIncreaseFactor = 2f;

    protected Rigidbody2D rigidbody2d;
    protected bool isGrounded;
    protected Vector2 targetVelocity;
    protected Vector2 groundNormal;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    private void OnEnable()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(_layerMask);
        contactFilter.useLayerMask = true;
    }

    private void Update()
    {
        targetVelocity = new Vector2(Input.GetAxis("Horizontal"), 0);

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            _animator.SetBool(_playerJump, true);
            _velocity.y = 7;
        }

        if (targetVelocity.x < 0 && _isFacingRight)
            Turn();
        else if (targetVelocity.x > 0 && _isFacingRight == false)
            Turn();
    }

    private void FixedUpdate()
    {
        _velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        _animator.SetBool(_playerRun, false);

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space) == false)
        {
            _animator.SetBool(_playerJump, false);
            _velocity.x = targetVelocity.x * _speed * _speedIncreaseFactor;
            _animator.SetBool(_playerRun, Mathf.Abs(targetVelocity.x) >= 0.1f);
        }
        else if (Input.GetKey(KeyCode.Space) == false)
        {
            _animator.SetBool(_playerJump, false);
            _velocity.x = targetVelocity.x * _speed;
            _animator.SetBool(_playerWalk, Mathf.Abs(targetVelocity.x) >= 0.1f);
        }

        isGrounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);
        move = Vector2.up * deltaPosition.y;
        Movement(move, true);
    }

    private void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rigidbody2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

            hitBufferList.Clear();

            for (int i = 0; i < count; i++)
                hitBufferList.Add(hitBuffer[i]);

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > _minGroundNormalY)
                {
                    isGrounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal);

                if (projection < 0)
                    _velocity -= projection * currentNormal;

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        rigidbody2d.position += move.normalized * distance;
    }

    private void Turn()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
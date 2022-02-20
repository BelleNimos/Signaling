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

    private const string Jump = "Jump";
    private const string Walk = "Walk";
    private const string Run = "Run";
    private const float MinGroundNormalY = 0.65f;
    private const float GravityModifier = 1f;
    private const float Speed = 3f;
    private const float SpeedIncreaseFactor = 2f;

    protected Rigidbody2D Rigidbody2d;
    protected bool IsGrounded;
    protected Vector2 TargetVelocity;
    protected Vector2 GroundNormal;
    protected ContactFilter2D ContactFilter;
    protected RaycastHit2D[] HitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> HitBufferList = new List<RaycastHit2D>(16);

    protected const float MinMoveDistance = 0.001f;
    protected const float ShellRadius = 0.01f;

    private void OnEnable()
    {
        Rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ContactFilter.useTriggers = false;
        ContactFilter.SetLayerMask(_layerMask);
        ContactFilter.useLayerMask = true;
    }

    private void Update()
    {
        TargetVelocity = new Vector2(Input.GetAxis("Horizontal"), 0);

        if (Input.GetKey(KeyCode.Space) && IsGrounded)
        {
            _animator.SetBool(Jump, true);
            _velocity.y = 7;
        }

        if (TargetVelocity.x < 0 && _isFacingRight)
            Turn();
        else if (TargetVelocity.x > 0 && _isFacingRight == false)
            Turn();
    }

    private void FixedUpdate()
    {
        _velocity += GravityModifier * Physics2D.gravity * Time.deltaTime;
        _animator.SetBool(Run, false);

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space) == false)
        {
            _animator.SetBool(Jump, false);
            _velocity.x = TargetVelocity.x * Speed * SpeedIncreaseFactor;
            _animator.SetBool(Run, Mathf.Abs(TargetVelocity.x) >= 0.1f);
        }
        else if (Input.GetKey(KeyCode.Space) == false)
        {
            _animator.SetBool(Jump, false);
            _velocity.x = TargetVelocity.x * Speed;
            _animator.SetBool(Walk, Mathf.Abs(TargetVelocity.x) >= 0.1f);
        }

        IsGrounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(GroundNormal.y, -GroundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Move(move, false);
        move = Vector2.up * deltaPosition.y;
        Move(move, true);
    }

    private void Move(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > MinMoveDistance)
        {
            int count = Rigidbody2d.Cast(move, ContactFilter, HitBuffer, distance + ShellRadius);

            HitBufferList.Clear();

            for (int i = 0; i < count; i++)
                HitBufferList.Add(HitBuffer[i]);

            for (int i = 0; i < HitBufferList.Count; i++)
            {
                Vector2 currentNormal = HitBufferList[i].normal;
                if (currentNormal.y > MinGroundNormalY)
                {
                    IsGrounded = true;
                    if (yMovement)
                    {
                        GroundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal);

                if (projection < 0)
                    _velocity -= projection * currentNormal;

                float modifiedDistance = HitBufferList[i].distance - ShellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        Rigidbody2d.position += move.normalized * distance;
    }

    private void Turn()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMovement : MonoBehaviour
{
    [SerializeField] private float _minGroundNormalY;
    [SerializeField] private float _gravityModifier;
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private LayerMask _layerMask;

    private Animator _animator;
    private float _speed = 3;
    private bool _facingRight = true;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(_layerMask);
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        targetVelocity = new Vector2(Input.GetAxis("Horizontal"), 0);

        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            _animator.SetBool("Jump", true);
            _velocity.y = 7;
        }

        if (targetVelocity.x < 0 && _facingRight)
            Turn();
        else if (targetVelocity.x > 0 && _facingRight == false)
            Turn();
    }

    void FixedUpdate()
    {
        _velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        _animator.SetBool("Run", false);

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space) == false)
        {
            _animator.SetBool("Jump", false);
            _velocity.x = targetVelocity.x * _speed * 2;
            _animator.SetBool("Run", Mathf.Abs(targetVelocity.x) >= 0.1f);
        }
        else if (Input.GetKey(KeyCode.Space) == false)
        {
            _animator.SetBool("Jump", false);
            _velocity.x = targetVelocity.x * _speed;
            _animator.SetBool("Walk", Mathf.Abs(targetVelocity.x) >= 0.1f);
        }

        grounded = false;

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
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

            hitBufferList.Clear();

            for (int i = 0; i < count; i++)
                hitBufferList.Add(hitBuffer[i]);

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > _minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal);

                if (projection < 0)
                    _velocity = _velocity - projection * currentNormal;

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }

    private void Turn()
    {
        _facingRight = !_facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
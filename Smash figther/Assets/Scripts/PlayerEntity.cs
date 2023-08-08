using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    [System.Serializable]

    public class FrictionsSettings
    {
        public float friction = 20f;
        public float turnAroundFriction = 20f;
    }

    //Move 
    [Header("Move")]
    public float aceleration = 20f;
    private float _dirX = 0f;
    public float speedMax = 10f;
    [HideInInspector]
    public float _speed = 0f;

    //Frictions
    [Header("Frictions")]
    public FrictionsSettings groundFriction;

    //gravity
    [Header("gravity")]
    public float gravity = 20f;
    public float fallSpeedMax = 10f;
    [HideInInspector]
    public float _verticalSpeed = 0f;

    //Ground
    [Header("Ground")]
    public bool _isOnground = false;

    //Jump
    [Header("Jump")]
    public float jumpSpeed = 5f;
    public float jumpDuration = 0.3f;
    private float _jumpCountdown = -1f;
    private bool _isJumping = false;

    //Orient
    private float _orientX = 1f;

    //RigidBody
    private Rigidbody2D _rigidbody;
    private BoxCollider2D boxCollider2D;
    public float distanceRay = 1f;

    private Animator anim;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        //anim.SetBool("IsIdle", true);
    }

    void Start()
    {
        _rigidbody.gravityScale = 0f;
    }

    void FixedUpdate()
    {
        if (_isJumping)
        {
            _UpdateJump();
        }
        else
        {
            _UpdateGravity();

            RaycastHit2D[] hit2D = Physics2D.LinecastAll(transform.position, transform.position + (Vector3.down) * distanceRay);

            Debug.DrawLine(transform.position, transform.position + (Vector3.down) * distanceRay);

            foreach (RaycastHit2D hit in hit2D)
            {
                if (hit.collider != null)
                {

                    if (hit.collider.CompareTag("Ground"))
                        _isOnground = true;
                }
            }

            RaycastHit2D[] hit2Dleft = Physics2D.LinecastAll(transform.position - new Vector3(0.25f, 0,0), transform.position - new Vector3(0.25f, 0, 0) + (Vector3.down) * distanceRay);

            Debug.DrawLine(transform.position - new Vector3(0.25f, 0, 0), transform.position - new Vector3(0.25f, 0, 0) + (Vector3.down) * distanceRay);

            foreach (RaycastHit2D hit in hit2Dleft)
            {
                if (hit.collider != null)
                {

                    if (hit.collider.CompareTag("Ground"))
                        _isOnground = true;
                }
            }

            RaycastHit2D[] hit2Dright = Physics2D.LinecastAll(transform.position + new Vector3(0.25f, 0, 0), transform.position + new Vector3(0.25f, 0, 0) + (Vector3.down) * distanceRay);

            Debug.DrawLine(transform.position + new Vector3(0.25f, 0, 0), transform.position + new Vector3(0.25f, 0, 0) + (Vector3.down) * distanceRay);

            foreach (RaycastHit2D hit in hit2Dright)
            {
                if (hit.collider != null)
                {

                    if (hit.collider.CompareTag("Ground"))
                        _isOnground = true;
                }
            }

            RaycastHit2D[] hit2Dup = Physics2D.LinecastAll(transform.position, transform.position + (Vector3.up) * distanceRay);

            Debug.DrawLine(transform.position, transform.position + (Vector3.up) * distanceRay);

            foreach (RaycastHit2D hit in hit2Dup)
            {
                if (hit.collider != null)
                {

                    if (hit.collider.CompareTag("Ground") /*|| hit.collider.CompareTag("DeadBottom")*/)
                    {
                        _jumpCountdown = 0f;
                        StopJump();
                        _UpdateGravity();
                    }
                }
            }
        }
        _UpdateMove();

        Vector2 velocity = Vector2.zero;
        velocity.x = _speed * _orientX;
        velocity.y = _verticalSpeed;
        _rigidbody.velocity = velocity;
    }

    #region Functions Gravity

    private void _UpdateGravity()
    {
        //if (_isOnground) return;

        _verticalSpeed -= gravity * Time.fixedDeltaTime;
        if (_verticalSpeed < -fallSpeedMax)
        {
            _verticalSpeed = -fallSpeedMax;
        }
    }
    #endregion

    #region Functions Ground

    public bool IsOnground()
    {
        return _isOnground;
    }

    #endregion

    #region Functions Move

    private void _UpdateMove()
    {
        Debug.Log(_dirX);
        if (_dirX != 0)
        {
            if (_dirX * _orientX < 0f)
            {
                _speed -= groundFriction.turnAroundFriction * Time.fixedDeltaTime;
                if (_speed <= 0f)
                {
                    _orientX = Mathf.Sign(_dirX);
                    _speed = 0f;
                }
            }
            else
            {
                //anim.SetBool("IsWalking", true);
                //anim.SetBool("IsIdle", false);
                _speed += aceleration * Time.fixedDeltaTime;
                if (_speed > speedMax)
                {
                    _speed = speedMax;
                }
                _orientX = Mathf.Sign(_dirX);
            }
        }
        else if (_speed > 0f)
        {
            _speed -= groundFriction.friction * Time.fixedDeltaTime;
            if (_speed < 0f)
            {
                _speed = 0;
                //anim.SetBool("IsWalking", false);
            }
        }
    }

    public void Move(float dirX)
    {
        _dirX = dirX;
    }

    #endregion

    #region Functions Jump

    public void Jump()
    {
        if (_isJumping) return;

        _isJumping = true;
        //anim.SetBool("IsJumping", true);

        _jumpCountdown = jumpDuration;
        _isOnground = false;
    }

    public bool IsJumping()
    {
        return _isJumping;
    }

    public void StopJump()
    {
        _isJumping = false;
        //anim.SetBool("IsJumping", false);
    }

    private void _UpdateJump()
    {
        if (!_isJumping) return;

        _jumpCountdown -= Time.fixedDeltaTime;
        if (_jumpCountdown < 0f)
        {
            StopJump();
        }
        else
        {
            _verticalSpeed = jumpSpeed;
        }
    }
    #endregion

    #region On trigger

    private void OnTriggerEnter2D(Collider2D other)
    {

    }

    private void OnTriggerExit2D(Collider2D other)
    {

    }
    
    #endregion
}
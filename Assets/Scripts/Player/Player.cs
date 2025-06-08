using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _anim;
    private CapsuleCollider2D _cd;
    
    [Header("Movement")]
    private float xInput;
    private float yInput;
    [SerializeField] private float moveSpeed;
    private bool _canBeControlled;
    [SerializeField] private float defaultGravityScale;
     
    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    private bool isAirBorne;
    private bool canDoubleJump;

    [Header("wall interactor")]
    [SerializeField] private bool isWallJump;
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private float wallJumpDuration = .5f;

    [Header("Jump Buffer & Coyote")]
    [SerializeField] private float jumpBufferTime = .2f;
    private float jumpBufferActive = -1;
    [SerializeField] private float coyoteTime = .2f;
    private float coyoteTimeActive = -1;

    [Header("Knock Back")]
    [SerializeField] private Vector2 knockBackForce;
    [SerializeField] private float knockBackDuration = .5f;
    private bool isKnockBack;


    [Header("Facing Direction")]
    [SerializeField] private bool facingRight = true;
    private int faceDirection = 1;


    [Header("Collision info")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float distanceToGround;
    [SerializeField] private float distanceToWall;
    private bool isGrounded;
    private bool isWallDetected;
    [Space]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float enemyCheckRadius = 0.1f;
    [SerializeField] private Transform enemyCheck;

    [Header("VFX")]
    [SerializeField] private GameObject DeathVFX;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _cd = GetComponent<CapsuleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultGravityScale = _rb.gravityScale;
        RespawnFinished(false);
    }

    // Update is called once per frame
    void Update()
    {

        UpdateAirBorneStatus();

        if (_canBeControlled == false){
            HandleAnimations();
            HandleCollison();
            return;
        
        }

        if (isKnockBack) return;

        HandleEnemyDetection();
        HandleInput();
        HandleWallSlide();
        HandleMovement();
        HandleFlip();
        HandleCollison();
        HandleAnimations();

    }

    private void HandleEnemyDetection()
    {
        if (_rb.linearVelocity.y >= 0) return;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(enemyCheck.position, enemyCheckRadius, enemyLayer);

        foreach (var enemy in enemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.Die();
                Jump();
            }
        }
    }

    public void RespawnFinished(bool finished)
    {
        if (finished) 
        {
            
            _rb.gravityScale = defaultGravityScale;
            _canBeControlled = true;
            _cd.enabled = true;
        }
        else
        {

            _rb.gravityScale = 0;
            _canBeControlled = false;
            _cd.enabled = false;
        }
    }

    public void Push(Vector2 pushDirection, float duration)
    {
        StartCoroutine(PushCoroutine(pushDirection, duration));
    }

    IEnumerator PushCoroutine(Vector2 pushDirection , float duration)
    {
        _canBeControlled = false;
        _rb.linearVelocity = Vector2.zero; // Reset velocity to prevent unwanted movement 
        _rb.AddForce(pushDirection, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);
        _canBeControlled = true;
    }

    public void KnockBack(float sourceDamagePosition)
    {
        float knockBackDirection = 1;
        if (transform.position.x < sourceDamagePosition)
            knockBackDirection = -1;

        if (isKnockBack) return;


        StartCoroutine(KnockBackCoroutine());
        _rb.linearVelocity = new Vector2(knockBackForce.x * knockBackDirection, knockBackForce.y);
        _anim.SetTrigger("KnockBack");
    }

    public void Die()
    {   
        GameObject deathVFX = Instantiate(DeathVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator KnockBackCoroutine()
    {
        isKnockBack = true;

        yield return new WaitForSeconds(knockBackDuration);

        isKnockBack = false;
    }

    private IEnumerator WallJumpCoroutine()
    {
        isWallJump = true;
        yield return new WaitForSeconds(wallJumpDuration);
        isWallJump = false;
    }

    private void WallJump()
    {
        isWallJump = true;
        _rb.linearVelocity = new Vector2(wallJumpForce.x * -faceDirection, wallJumpForce.y);
        Flip();

        StartCoroutine(WallJumpCoroutine());
    }

    private void HandleWallSlide()
    {
        bool canWallSlide = isWallDetected && !isGrounded && _rb.linearVelocity.y < 0;
        float yModifier = 0.5f;
        if (!canWallSlide) return;

        if (yInput < 0)
        {
            yModifier = 1f;
        }

        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * yModifier);
    }

    private void UpdateAirBorneStatus()
    {
        if (isGrounded && isAirBorne)
            HandleLanding();
        
        if (!isGrounded && !isAirBorne)
            BecomeAirborne();
    }

    private void BecomeAirborne()
    {
        isAirBorne = true;

        ActiveCoyote();
    }

    private void HandleLanding()
    {
        isAirBorne = false;
        canDoubleJump = true;

        AttempBufferJump();
    }

    private void RequestBufferJump()
    {
        if (isAirBorne)
        {
            jumpBufferActive = Time.time;
        }
    }


    private void AttempBufferJump()
    {
        if(Time.time < jumpBufferActive + jumpBufferTime)
        {
            Jump();
            jumpBufferActive = Time.time -1;
        }
    }

    private void ActiveCoyote() => coyoteTimeActive = Time.time;
    private void DeactiveCoyote() => coyoteTimeActive = Time.time - 1;


    private void HandleFlip()
    {
        if ((xInput < 0 && facingRight) || (xInput > 0 && !facingRight))
            Flip();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }

        
    }
    private void JumpButton()
    {
        bool coyoteValid = Time.time < coyoteTimeActive + coyoteTime;
        if (isGrounded || coyoteValid)
        {
            Jump();
        }
        else if(isWallDetected)
        {
             WallJump();
        }
        else if (canDoubleJump)
        {

            DoubleJump();
        }

        DeactiveCoyote();
    }

    private void DoubleJump()
    {
        StopCoroutine(WallJumpCoroutine());
        isWallJump = false;
        canDoubleJump = false;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
    }

    private void Jump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
    }

    private void HandleCollison()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, groundLayer);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * faceDirection, distanceToWall, groundLayer);
    }

    private void HandleAnimations()
    {
        _anim.SetFloat("xVelocity", _rb.linearVelocity.x);
        _anim.SetFloat("yVelocity", _rb.linearVelocity.y);
        _anim.SetBool("isGrounded", isGrounded);
        _anim.SetBool("isWallDetected", isWallDetected);
    }

    private void Flip()
    {
        faceDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void HandleMovement()
    {
        if (isWallDetected || isWallJump) return;

        _rb.linearVelocity = new Vector2(xInput * moveSpeed, _rb.linearVelocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + distanceToWall*faceDirection, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - distanceToGround));
    }
}

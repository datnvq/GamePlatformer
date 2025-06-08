using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer _sr => GetComponent<SpriteRenderer>();
    protected Animator _anim;
    protected Rigidbody2D _rb;
    protected BoxCollider2D[] _cd;
    protected Transform playerTransform;
    [Space]
    [Space]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected int faceDirection = -1; // -1 for left, 1 for right
    [SerializeField] protected float timerDuration = 1.5f;
    [SerializeField] protected bool canMove = false;

    [Header("Death Details")]
    [SerializeField] protected float deathImpactSpeed = 5f;
    [SerializeField] protected float deathRotationSpeed = 150f;
    protected int deathRotationDirection = 1;
    protected bool isDead = false; // Used to check if the enemy is dead

    [Header("Collision")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float distanceToGround = 1.1f;
    [SerializeField] protected float distanceToWall = 0.9f;
    [SerializeField] protected float playerDetectionDistance = 15f;
    protected bool playerDetected;
    protected bool isGroundedDetected; // Used to check if the enemy is grounded
    protected bool isGroundedInFrontDetected;
    protected bool isWallDetected;
    protected bool facingRight = false; // true for right, false for left


    protected virtual void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _cd = GetComponentsInChildren<BoxCollider2D>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InvokeRepeating(nameof(UpdatePlayerRef), 0, 1);

        if(_sr.flipX == true && !facingRight)
        {
            _sr.flipX = false; // Reset the sprite flip state to match the facing direction
            Flip(); // Ensure the initial facing direction matches the sprite's flip state
        }
    }

    private void UpdatePlayerRef()
    {
        if(playerTransform == null)
        {
            playerTransform = GameManager.Instance.player.transform;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleCollison();
        HandleAnimator();
        if (isDead)
            HandleDeathRotation();
    }
    protected virtual void HandleAnimator()
    {
        _anim.SetFloat("xVelocity", _rb.linearVelocity.x);
    }
    public virtual void Die()
    {
        foreach (var collider in _cd)
        {
            collider.enabled = false; // Disable all colliders on death
        }
        _anim.SetTrigger("Hit");
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, deathImpactSpeed);
        isDead = true;
        deathRotationDirection = Random.Range(0, 100) < 50 ? 1 : -1;
        Destroy(gameObject, 5f); // Destroy the enemy after 3 seconds
    }
    private void HandleDeathRotation()
    {
        transform.Rotate(0, 0, (deathRotationSpeed * deathRotationDirection) * Time.deltaTime);
    }
    protected virtual void HandleFlip(float xValue)
    {
        if ((xValue < 0 && facingRight) || (xValue > 0 && !facingRight))
            Flip();
    }
    protected virtual void Flip()
    {
        faceDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    [ContextMenu("Flip Default Facing Direction")]
    public void FlipDefaultFacingDirection()
    {
        _sr.flipX = !_sr.flipX;
    }
    protected virtual void HandleCollison()
    {
        isGroundedDetected = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, groundLayer);
        isGroundedInFrontDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, distanceToGround, groundLayer);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * faceDirection, distanceToWall, groundLayer);
        playerDetected = Physics2D.Raycast(transform.position, Vector2.right * faceDirection, playerDetectionDistance, playerLayer);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + playerDetectionDistance * faceDirection, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - distanceToGround));
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - distanceToGround));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + distanceToWall * faceDirection, transform.position.y));
    }

    protected virtual void TimerDuration()
    {
        StartCoroutine(TimeDurationCoroutine());
    }

    private IEnumerator TimeDurationCoroutine()
    {
        canMove = false;
        yield return new WaitForSeconds(timerDuration);
        canMove = true;
    }
}

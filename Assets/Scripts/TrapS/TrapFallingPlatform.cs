using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFallingPlatform : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _anim;
    private BoxCollider2D[] _collider;

    public bool canMove;


    public Vector3[] wayPoint;
    public float travelDistance = .6f;
    public float yOffset;
    public int waypointIndex = 0;

    [Space]

    [Header("Impact Fall detail")]
    [SerializeField] private float impactSpeed = 3f;
    [SerializeField] private float impactDuration = 0.5f;
    [SerializeField] private float _impactTimer;
    [SerializeField] private bool isImpacting;
    public float fallDelay = 2f;
    public float fallSpeed;
    


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _collider = GetComponents<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetWayPoint();
        float randomFirstTimeActive = Random.Range(0, .6f); // Randomize the delay time between 1 and 3 seconds
        Invoke(nameof(CanMoveTrue), randomFirstTimeActive);
    }

    private void CanMoveTrue() => canMove = true;

    private void GetWayPoint()
    {
        wayPoint = new Vector3[2];
        yOffset = travelDistance / 2; // Adjust this value to set how far down the platform should fall
        wayPoint[0] = new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z);
        wayPoint[1] = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        HandleImpact();
        HandleMovement();
    }

    private void HandleImpact()
    {
        if (_impactTimer < 0) return;
        _impactTimer -= Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + (Vector3.down*10) , impactSpeed * Time.deltaTime);
    }
    
    private void HandleMovement()
    {
        if(!canMove) return;

        transform.position = Vector3.MoveTowards(transform.position, wayPoint[waypointIndex], fallSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, wayPoint[waypointIndex]) < 0.01f)
        {
            ++waypointIndex;

            if (waypointIndex >= wayPoint.Length)
            {
                waypointIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isImpacting) return;

        Player player = collision.GetComponent<Player>();

        if (player)
        {
            Invoke(nameof(ActiveFall), fallDelay);
            _impactTimer = impactDuration; // Start the impact timer
            isImpacting = true;
        }
    }

    private void ActiveFall()
    {
        _anim.SetBool("Active", true);
        canMove = false; // Stop the platform from moving horizontally
        _rb.bodyType = RigidbodyType2D.Kinematic; // Allow the platform to fall
        _rb.gravityScale = 3.5f; // Set gravity scale to allow falling
        _rb.linearDamping = 0.05f;
        foreach (var col in _collider)
        {
            col.enabled = false; // Disable the colliders to allow the platform to fall through
        }
    }
}

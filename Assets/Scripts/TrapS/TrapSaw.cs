using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSaw : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _sr;
    private TrapSawWayPoint _trapSawWayPoint;

    public float moveSpeed;
    public int moveDirection = 1; // 1 for right, -1 for left
    public float delayTime;
    public bool canMove = true;
    

    [Header("Waypoints")]
    public int wayPointIndex = 1;
    public Transform[] wayPoints;
    public Vector3[] wayPointPosition;



    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        UpdateWayPointPosition();
        transform.position = wayPointPosition[0];
    }

    private void UpdateWayPointPosition()
    {
        List<TrapSawWayPoint> wayPointsList = new List<TrapSawWayPoint>(GetComponentsInChildren<TrapSawWayPoint>());
        if(wayPointsList.Count != wayPoints.Length)
        {
            wayPoints = new Transform[wayPointsList.Count];

            for (int i = 0; i < wayPointsList.Count; i++)
            {
                wayPoints[i] = wayPointsList[i].transform;
            }
        }
        wayPointPosition = new Vector3[wayPoints.Length];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPointPosition[i] = wayPoints[i].position;
            Destroy(wayPoints[i].gameObject); // Destroy the waypoint GameObjects if they are not needed anymore
        }
    }

    private void Update()
    {
        _anim.SetBool("Active", canMove);

        if (canMove == false) return;

        transform.position = Vector2.MoveTowards(transform.position, wayPointPosition[wayPointIndex], moveSpeed*Time.deltaTime);

        if(Vector2.Distance(transform.position, wayPointPosition[wayPointIndex]) < 0.01f)
        {
            if(wayPointIndex == wayPoints.Length - 1 || wayPointIndex == 0)
            {
                moveDirection *= -1;
                StartCoroutine(DelayMovement());
            }
            wayPointIndex += moveDirection;

        }
    }

    private IEnumerator DelayMovement()
    {
        canMove = false;

        yield return new WaitForSeconds(delayTime);

        _sr.flipX = !_sr.flipX;
        canMove = true;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chicken : Enemy
{
    [Header("Chicken Specific Settings")]
    [SerializeField] private float aggroDuration = 2f; // Time before the chicken turns around
    private float aggroTimer;

    private bool canFlip = true; // To control flipping behavior

    protected override void Update()
    {
        base.Update();
        aggroTimer -= Time.deltaTime;

        if (isDead)
        {
            return;
        }

        if(playerDetected) 
        { 
            canMove = true;
            aggroTimer = aggroDuration; // Reset the aggro timer when player is detected
        }

        if(aggroTimer < 0) canMove = false; // Stop movement after aggro duration

        HandleMovement();

        if (isGroundedDetected == false) return;
        HandleTurnAround();
    }

    private void HandleTurnAround()
    {
        if (!isGroundedInFrontDetected || isWallDetected)
        {
            Flip();
            canMove = false; // Stop movement when turning around
        }
    }

    private void HandleMovement()
    {
        if (canMove == false)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        HandleFlip(playerTransform.position.x);

        _rb.linearVelocity = new Vector2(moveSpeed * faceDirection, _rb.linearVelocity.y);
    }

    protected override void HandleFlip(float xPosPlayer)
    {
        if (xPosPlayer < transform.position.x && facingRight || xPosPlayer > transform.position.x && !facingRight)
        {
            if (canFlip)
            {
                canFlip = false; // Prevent immediate flipping again
                Invoke(nameof(Flip), 0.5f); // Delay the flip to allow for smoother transitions
            }
        }
    }

    protected override void Flip()
    {
        base.Flip();

        canFlip = true; // Allow flipping again after the delay
    }

}

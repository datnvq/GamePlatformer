using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rino : Enemy
{
    [Header("Rino Specific Settings")]
    [SerializeField] private float maxSpeed = 5f; // Max Speed of the Rino
    [SerializeField] private float speedUpRate = 0.7f; // Rate at which the Rino speeds up
    private float defaultSpeed = 5; // Default Move Speed
    [SerializeField] private Vector2 impactPower;
    


    protected override void Start()
    {
        base.Start();

        defaultSpeed = moveSpeed; // Set default speed to initial move speed
    }

    protected override void Update()
    {
        base.Update();
        HandleCharge();
    }

    private void HandleCharge()
    {
        if (canMove == false) return;
        HandleSpeedup();

        _rb.linearVelocity = new Vector2(moveSpeed * faceDirection, _rb.linearVelocity.y);

        if (isWallDetected)
        {
            HitWall();
        }

        if (!isGroundedInFrontDetected)
        {
            TurnAround();
        }
    }

    private void HandleSpeedup()
    {
        moveSpeed = moveSpeed + (speedUpRate * Time.deltaTime); // Increase speed over time

        if (moveSpeed > maxSpeed)
        {
            moveSpeed = maxSpeed; // Clamp speed to max speed
        }
    }

    private void TurnAround()
    {
        ResetSpeed(); // Reset move speed to default
        canMove = false;
        _rb.linearVelocity = Vector2.zero; // Stop movement if not grounded in front
        Flip(); // Flip the direction if not grounded in front
    }

    protected override void HandleCollison()
    {
        base.HandleCollison();


        if (playerDetected && isGroundedDetected)
        {
            canMove = true;
        }

    }

    private void ChargeIsOver()
    {
        _anim.SetBool("HitWall", false);
        Invoke(nameof(Flip), 1);
    }

    private void HitWall()
    {
        canMove = false;
        ResetSpeed();
        _anim.SetBool("HitWall", true);
        _rb.linearVelocity = new Vector2(impactPower.x * -faceDirection, impactPower.y);
    }

    private void ResetSpeed()
    {
        moveSpeed = defaultSpeed;
    }
}

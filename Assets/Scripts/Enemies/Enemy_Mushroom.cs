using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mushroom : Enemy
{

    protected override void Update()
    {
        base.Update();

        if (isDead)
        {
            return;
        }
        HandleMovement();

        if (isGroundedDetected == false) return;
        HandleTurnAround();
    }

    private void HandleTurnAround()
    {
        if (!isGroundedInFrontDetected || isWallDetected)
        {
            Flip();
            TimerDuration();
        }
    }

    private void HandleMovement()
    {
        if(canMove == false)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }
        _rb.linearVelocity = new Vector2(moveSpeed * faceDirection, _rb.linearVelocity.y);
    }


}

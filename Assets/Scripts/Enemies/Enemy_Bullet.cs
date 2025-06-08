using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(Vector2 velocity)
    {
        if (_rb == null) return;

        _rb.linearVelocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_rb == null) return;
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().KnockBack(transform.position.x); // Assuming Player has a TakeDamage method
            Destroy(gameObject); // Destroy the bullet on hit
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject, .05f); // Destroy the bullet on hit
        }
    }
}

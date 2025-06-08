using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikedBall : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private bool _rightDirection = true;
    void Start()
    {
        Vector2 direction = _rightDirection ? Vector2.right : Vector2.left;
        _rb.AddForce(direction * _speed, ForceMode2D.Impulse);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrampoline : MonoBehaviour
{
    protected Animator _anim;

    public float pushPower = 10f;
    public float pushDuration = 0.5f;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            _anim.SetTrigger("Active");
            player.Push(transform.up * pushPower, pushDuration);
        }
    }
}

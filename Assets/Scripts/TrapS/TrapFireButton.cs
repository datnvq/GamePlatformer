using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFireButton : MonoBehaviour
{
    private TrapFire _trapFire;
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _trapFire = GetComponentInParent<TrapFire>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            _anim.SetTrigger("Active");
            _trapFire.SwitchFire();
        }
    }


}

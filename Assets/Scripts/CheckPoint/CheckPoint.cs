using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    private bool _activated = false;
    private bool _canBeReactive;

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _canBeReactive = GameManager.Instance.CanBeReactive();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        
        if (_activated && _canBeReactive) return;

        if(player != null)
        {
            ActivateCheckPoint();
            GameManager.Instance.UpdateRespawnPlayer(transform);
        }
    }

    private void ActivateCheckPoint()
    {
        _anim.SetTrigger("Active");
        _activated = true;
    }
}

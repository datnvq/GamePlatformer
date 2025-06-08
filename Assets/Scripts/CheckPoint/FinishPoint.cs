using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if(player != null)
        {
            _anim.SetTrigger("Active");
            GameManager.Instance.LevelFinished();
        }
    }

}

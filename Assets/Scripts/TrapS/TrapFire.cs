using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapFire : MonoBehaviour
{
    public float offDuration = 2f;
    [SerializeField] private Animator _anim;
    [SerializeField] private CapsuleCollider2D _cs;
    private TrapFireButton _trapFireButton;
    public bool isActive;    

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _cs = GetComponent<CapsuleCollider2D>();
        _trapFireButton = GetComponentInChildren<TrapFireButton>();
    }

    private void Start()
    {
        if (_trapFireButton == null)
            Debug.LogError("TrapFireButton is not assigned in the inspector.");

        SetFire(true);
    }

    public void SwitchFire()
    {
        if(!isActive) return;
        StartCoroutine(FireCoroutine());

    }

    IEnumerator FireCoroutine()
    {
        SetFire(false);
        yield return new WaitForSeconds(offDuration);
        SetFire(true);
    }

    private void SetFire(bool active)
    {
        _anim.SetBool("Active", active);
        _cs.enabled = active;
        isActive = active;
    }

    
}

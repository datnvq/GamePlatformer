using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrow : TrapTrampoline
{
    [SerializeField] private bool _rightDirection;

    public float reSpawnTime = 2;

    [SerializeField] private float _growSpeed = 1f;
    [SerializeField] private Vector3 _maxScale;


    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleGrowScale();
        HandleRotationDirect();
    }

    private void HandleGrowScale()
    {
        if (transform.localScale.x < _maxScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _maxScale, _growSpeed * Time.deltaTime);
        }
    }

    private void HandleRotationDirect()
    {
        int direction = _rightDirection ? 1 : -1;
        transform.Rotate(0, 0, direction * 120f * Time.deltaTime);
    }

    public void DestroyMe()
    {
        GameManager.Instance.SpawnArraw(transform.position, reSpawnTime);
        Destroy(gameObject);
    }

}

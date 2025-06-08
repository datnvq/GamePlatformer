using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{
    [Header("Plant Specific Settings")]
    [SerializeField] private Enemy_Bullet bulletPrefab; // Reference to the bullet prefab
    [SerializeField] private float bulletSpeed = 5f; // Speed of the bullet
    [SerializeField] private Transform bulletSpawnPoint; // Point where the bullet will be spawned
    [SerializeField] private float attackCooldown = 2f; // Time between attacks
    private float lastTimeAttack;
    protected override void Update()
    {
        base.Update();

        bool canAttack = Time.time >= lastTimeAttack + attackCooldown;
        if (playerDetected && canAttack)
            Attack();
    }

    private void Attack()
    {
        lastTimeAttack = Time.time;
        _anim.SetTrigger("Attack");
    }

    private void SpawnBullet()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            Enemy_Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Vector2 bulletVelocity = new Vector2(faceDirection * bulletSpeed, 0); // Set bullet speed and direction
            bullet.SetVelocity(bulletVelocity);

            Destroy(bullet.gameObject, 10f); // Destroy bullet after 5 seconds to prevent memory leaks
        }
    }

    protected override void HandleAnimator()
    {
        //Để trống, vì trong Enemy_Plant không cần xử lý animator
    }
}

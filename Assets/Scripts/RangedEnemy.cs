using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Patrol")]
    public Transform leftPoint;
    public Transform rightPoint;
    public float patrolSpeed = 2f;

    [Header("Shoot")]
    public GameObject bulletPrefab;
    public float shootRange = 5f;
    public float shootCooldown = 2f;

    private Transform player;
    private float patrolDirection = 1f;
    private float shootTimer;

    private float leftBound, rightBound;

    private void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
        leftBound = leftPoint.position.x;   // Store WORLD positions once
        rightBound = rightPoint.position.x;
    }

    private void Update()
    {
        // Patrol
        transform.Translate(Vector2.right * patrolDirection * patrolSpeed * Time.deltaTime);

        if (transform.position.x <= leftBound)
            patrolDirection = 1f;
        if (transform.position.x >= rightBound)
            patrolDirection = -1f;

        // Shoot logic
        if (player != null &&
            Vector2.Distance(transform.position, player.position) < shootRange &&
            Time.time > shootTimer)
        {
            Shoot();
            shootTimer = Time.time + shootCooldown;
        }
    }
    
    void Shoot()
    {
        // Play Wwise ranged_shoot
        AkUnitySoundEngine.PostEvent("PlayRangedShoot", gameObject);

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().speed = 8f;
    }
}

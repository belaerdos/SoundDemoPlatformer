using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject tornadoPrefab;  // PlayerTornado prefab with its script
    public Transform firePoint;      // PlayerFirepoint
    public float tornadoSpeed = 10f;
    public float fireRate = 0.5f;

    private float nextFireTime = 0f;

    void Start()
    {
        if (firePoint == null) firePoint = transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextFireTime)
        {
            GameObject targetEnemy = FindNearestEnemyWithHealth();
            if (targetEnemy != null)
            {
                Shoot(targetEnemy.transform.position);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    GameObject FindNearestEnemyWithHealth()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float closestDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null && health.currentHealth > 0)  // Direct reference
            {
                float dist = Vector2.Distance(transform.position, enemy.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = enemy;
                }
            }
        }
        return closest;
    }

    void Shoot(Vector3 targetPos)
    {
        // Play Wwise tornado
        AkUnitySoundEngine.PostEvent("PlayTornado", gameObject);

        Vector2 direction = (targetPos - firePoint.position).normalized;
        GameObject tornado = Instantiate(tornadoPrefab, firePoint.position, Quaternion.identity);
        tornado.GetComponent<PlayerTornado>().speed = tornadoSpeed;

        Debug.Log("Tornado fired!");
    }
}


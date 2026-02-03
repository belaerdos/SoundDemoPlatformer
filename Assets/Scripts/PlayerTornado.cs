using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTornado : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Auto cleanup
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(1);  // 1 damage per tornado
            }

            Debug.Log("Enemy hit!");
            Destroy(gameObject);
        }
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}

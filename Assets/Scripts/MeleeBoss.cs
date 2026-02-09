using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBoss : MonoBehaviour
{
    [Header("Attack")]
    public float attackRange = 1.5f;
    public float attackCooldown = 3f;
    public float lungeSpeed = 8f;
    public float lungeDuration = 0.4f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private float nextAttackTime;
    private bool isAttacking;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking) return;

        // Face player
        float dirX = player.position.x - transform.position.x;
        transform.localScale = new Vector3(Mathf.Sign(dirX), 1, 1);

        // Check range + cooldown
        if (Vector2.Distance(transform.position, player.position) <= attackRange &&
            Time.time >= nextAttackTime)
        {
            StartCoroutine(Lunge());
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    System.Collections.IEnumerator Lunge()
    {
        isAttacking = true;
        anim.SetBool("isLunging", true);

        Vector2 lungeDir = (player.position - transform.position).normalized;
        rb.velocity = lungeDir * lungeSpeed;

        yield return new WaitForSeconds(lungeDuration);

        rb.velocity = Vector2.zero;
        anim.SetBool("isLunging", false);
        isAttacking = false;
    }

    public void DealDamageToPlayer()
    {
        if (player == null) return;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }
    }
}



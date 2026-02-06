using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonEnemy : MonoBehaviour
{
    public enum DragonState { Idle, Chasing, Attacking }

    [Header("General")]
    public float detectionRange = 8f;
    public float moveSpeed = 3f;
    public float attackCooldown = 2f;

    [Header("Attacks")]
    public float flyKickRange = 1.3f;
    public float jumpAttackRange = 1.3f;

    private Transform player;
    private EnemyHealth health;
    private Animator anim;
    private Rigidbody2D rb;

    private DragonState state = DragonState.Idle;
    private float nextAttackTime;
    private bool useJumpAttack;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        health = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null || health == null) return;

        // Switch attack after losing 2 HP
        if (health.currentHealth <= health.maxHealth - 2)
            useJumpAttack = true;

        float dist = Vector2.Distance(transform.position, player.position);

        FacePlayer();

        switch (state)
        {
            case DragonState.Idle:
                HandleIdle(dist);
                break;

            case DragonState.Chasing:
                HandleChasing(dist);
                break;

            case DragonState.Attacking:
                // Movement handled in coroutine
                rb.velocity = Vector2.zero;
                break;
        }
    }

    void HandleIdle(float dist)
    {
        anim.Play("dragonEnemyIdle");
        rb.velocity = Vector2.zero;

        if (dist <= detectionRange && Time.time >= nextAttackTime)
        {
            state = DragonState.Chasing;
        }
    }

    void HandleChasing(float dist)
    {
        float attackRange = useJumpAttack ? jumpAttackRange : flyKickRange;

        // Too far → stop chasing
        if (dist > detectionRange)
        {
            state = DragonState.Idle;
            return;
        }

        // In attack range
        if (dist <= attackRange)
        {
            // Cooldown active → wait (idle, no movement)
            if (Time.time < nextAttackTime)
            {
                state = DragonState.Idle;
                return;
            }

            // Ready to attack
            StartCoroutine(AttackRoutine());
            return;
        }

        // Move toward player
        anim.Play("dragonEnemyWalk");
        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
    }

    IEnumerator AttackRoutine()
    {
        state = DragonState.Attacking;
        rb.velocity = Vector2.zero;
        health.canTakeDamage = false;

        if (!useJumpAttack)
        {
            anim.Play("dragonEnemyFlyKick");
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            anim.Play("dragonEnemyJumpAttack");
            yield return new WaitForSeconds(0.7f);
        }

        health.canTakeDamage = true;
        nextAttackTime = Time.time + attackCooldown;
        state = DragonState.Idle;
    }

    void FacePlayer()
    {
        float dirX = player.position.x - transform.position.x;
        if (dirX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(dirX), 1f, 1f);
        }
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


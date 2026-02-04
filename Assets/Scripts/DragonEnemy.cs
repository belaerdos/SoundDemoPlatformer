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
    public float flyKickRange = 2f;
    public float jumpAttackRange = 3f;

    private Transform player;
    private EnemyHealth health;
    private Animator anim;
    private Rigidbody2D rb;

    private DragonState state = DragonState.Idle;
    private bool isAttacking;
    private float nextAttackTime;
    private bool useJumpAttack;   // switches after 2 HP lost

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null || health == null) return;

        // Switch to JumpAttack after losing 2 HP (4 → 2)
        if (health.currentHealth <= health.maxHealth - 2)
            useJumpAttack = true;

        if (isAttacking) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // Face player
        float dirX = player.position.x - transform.position.x;
        transform.localScale = new Vector3(Mathf.Sign(dirX), 1f, 1f);

        switch (state)
        {
            case DragonState.Idle:
                anim.Play("dragonEnemyIdle");
                if (dist <= detectionRange)
                    state = DragonState.Chasing;
                break;

            case DragonState.Chasing:
                anim.Play("dragonEnemyWalk"); // walk animation

                // Move toward player
                Vector2 dir = (player.position - transform.position).normalized;
                rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);

                // Decide when to attack
                float neededRange = useJumpAttack ? jumpAttackRange : flyKickRange;
                if (dist <= neededRange && Time.time >= nextAttackTime)
                    StartCoroutine(AttackRoutine());
                // If player far again, go back to Idle
                if (dist > detectionRange * 1.2f)
                    state = DragonState.Idle;
                break;
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;
        state = DragonState.Attacking;

        // Make dragon invulnerable during attack
        health.canTakeDamage = false;

        if (!useJumpAttack)
        {
            // Stage 1: FlyKick
            anim.Play("dragonEnemyFlyKick");
            // TODO: move forward a bit or deal damage via animation event
            yield return new WaitForSeconds(0.5f); // match animation length
        }
        else
        {
            // Stage 2: JumpAttack
            anim.Play("dragonEnemyJumpAttack");
            // TODO: vertical/horizontal leap, damage via event
            yield return new WaitForSeconds(0.7f); // match animation length
        }

        // Back to idle/chase
        anim.Play("dragonEnemyIdle");
        health.canTakeDamage = true;   // vulnerable again
        isAttacking = false;
        state = DragonState.Chasing;
        nextAttackTime = Time.time + attackCooldown;
    }
}

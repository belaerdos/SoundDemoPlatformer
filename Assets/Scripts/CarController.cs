using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float carSpeed = 5f;
    private Rigidbody2D rb;
    private bool occupied = false;
    private Transform player;
    private Vector2 startPlayerPos; // Optional: save player pos on exit

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !occupied)
        {
            player = other.transform;
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            playerRb.isKinematic = true; // Prevent fall/physics fight
            player.SetParent(transform);
            occupied = true;
        }
    }

    void Update()
    {
        if (!occupied || player == null) return;

        // Drive right with D/right arrow
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(carSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Stop on release
        }

        // Exit with F to right side
        if (Input.GetKeyDown(KeyCode.F))
        {
            player.SetParent(null);
            player.position = transform.position + Vector3.right * 1.5f; // Exit right
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            playerRb.isKinematic = false; // Restore physics
            occupied = false;
            player = null;
        }
    }
}


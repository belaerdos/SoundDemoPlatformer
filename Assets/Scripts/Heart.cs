using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public int heartValue = 1;
    public Transform player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == null) return;

        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.AddHealth(heartValue);
            }

            gameObject.SetActive(false);
        }
    }
}

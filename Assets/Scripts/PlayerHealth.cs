using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthDisplay();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void AddHealth(int healthValue)
    {
        currentHealth += healthValue;
        UpdateHealthDisplay();
    }

    void Die()
    {
        Debug.Log("Player died!");
        // Respawn or game over later

        gameObject.SetActive(false);  // Deactivates and hides the GameObject
    }

    public void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();

            // Color logic based on health
            if (currentHealth >= 8)
                healthText.color = Color.white;
            else if (currentHealth >= 5)
                healthText.color = Color.yellow;
            else if (currentHealth >= 3)
                healthText.color = Color.red;
            else if (currentHealth >= 1)
                healthText.color = Color.black;
            else
                healthText.color = Color.gray; // Dead state
        }
    }
}

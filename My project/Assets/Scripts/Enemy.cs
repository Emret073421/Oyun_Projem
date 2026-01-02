using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int damageToPlayer = 20; // Sana dokununca kaç vursun?
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // --- SEN ONA VURUNCA ÇALIÞIR ---
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Düþman hasar aldý! Düþman Kalan Can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // --- O SANA DOKUNUNCA ÇALIÞIR (YENÝ) ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Çarptýðý þeyin etiketi "Player" mý?
        if (collision.gameObject.CompareTag("Player"))
        {
            // O objenin üzerindeki PlayerController kodunu al
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            // Eðer kodu bulduysa hasar ver
            if (player != null)
            {
                player.TakeDamage(damageToPlayer);
            }
        }
    }

    void Die()
    {
        Debug.Log("Düþman ÖLDÜ!");
        Destroy(gameObject);
    }
}
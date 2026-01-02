using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Oyun yeniden başlasın diye

public class PlayerController : MonoBehaviour
{
    [Header("Karakter Ayarları")]
    public float moveSpeed = 5f;
    public int maxHealth = 100; // Senin canın
    private int currentHealth;

    [Header("Saldırı Ayarları")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private Vector2 lastDirection = Vector2.down;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth; // Oyuna tam canla başla
    }

    void Update()
    {
        // Hareket
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.sqrMagnitude > 0)
        {
            lastDirection = movement;
            // Attack point yönünü ayarla (0.2f mesafe)
            attackPoint.localPosition = lastDirection.normalized * 0.2f;

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        // Saldırı Tuşu
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        // Doğru olan bu:
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // --- SALDIRI FONKSİYONU ---
    void Attack()
    {
        animator.SetTrigger("Attack");

        // Kılıcın değdiği her şeyi bul
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Çarptığım şeyde "CowAI" scripti var mı diye bak
            CowAI inekScripti = enemy.GetComponent<CowAI>();

            if (inekScripti != null)
            {
                // Varsa hasar ver
                inekScripti.HasarAl(attackDamage);
            }
        }
    }

    // --- SENİN HASAR ALMA FONKSİYONUN (YENİ) ---
    // --- GÜNCELLENMİŞ HASAR ALMA FONKSİYONU ---
    public void TakeDamage(int damage)
    {
        // Hasarı canımızdan düş
        currentHealth -= damage;

        // Konsola detaylı rapor yaz (Örn: "Ah! 10 hasar aldım. Kalan Can: 90")
        Debug.Log($"⚠️ DİKKAT: {damage} hasar alındı! | Kalan Can: {currentHealth}");

        // Öldün mü kontrol et
        if (currentHealth <= 0)
        {
            Debug.Log("💀 KARAKTER ÖLDÜ! OYUN BİTTİ.");

            // Sahneyi yeniden başlat
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
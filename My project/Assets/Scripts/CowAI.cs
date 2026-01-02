using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowAI : MonoBehaviour
{
    [Header("Can Ayarları")]
    public int maxCan = 100;
    private int guncelCan;

    [Header("Hareket Ayarları")]
    public float yurumeHizi = 1.5f;
    public float kosmaHizi = 3.5f;
    public float gorusMesafesi = 5f;
    public float durmaMesafesi = 1f;
    public int hasarGucu = 10; // Sana vurduğunda senin canını ne kadar acıtsın?

    private Transform player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool beniGorduMu = false;
    private bool suanKosuyor = false;
    private float aramaZamanlayicisi;

    void Start()
    {
        guncelCan = maxCan; // Oyuna tam canla başla

        PlayerController pc = FindObjectOfType<PlayerController>();
        if (pc != null) player = pc.transform;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        aramaZamanlayicisi = Random.Range(1f, 3f);
    }

    void Update()
    {
        if (player == null) return;

        float mesafe = Vector2.Distance(transform.position, player.position);

        // --- HAREKET MANTIĞI ---
        if (mesafe < gorusMesafesi)
        {
            if (!beniGorduMu)
            {
                beniGorduMu = true;
                StartCoroutine(HeyecanlanVeKos());
            }

            if (mesafe > durmaMesafesi)
            {
                YonuKaraktereCevir();
                HareketEt();
                animator.SetFloat("Speed", 1);

                Vector2 dir = (player.position - transform.position).normalized;
                animator.SetFloat("Horizontal", dir.x);
                animator.SetFloat("Vertical", dir.y);
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0);
            beniGorduMu = false;
            EtrafiKolacanEt();
        }
    }

    // --- SEN İNEĞE VURUNCA BU ÇALIŞACAK ---
    public void HasarAl(int hasar)
    {
        guncelCan -= hasar;
        Debug.Log($"İnek hasar aldı! Kalan Canı: {guncelCan}");

        if (guncelCan <= 0)
        {
            Oldu();
        }
    }

    void Oldu()
    {
        Debug.Log("MÖÖÖ! BİR İNEK ÖLDÜ! 💀");
        Destroy(gameObject); // İneği sahneden sil
    }

    // --- İNEK SANA ÇARPINCA BU ÇALIŞACAK ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(hasarGucu);
            }
        }
    }

    // --- YARDIMCI FONKSİYONLAR ---
    void HareketEt()
    {
        float aktifHiz = suanKosuyor ? kosmaHizi : yurumeHizi;
        transform.position = Vector2.MoveTowards(transform.position, player.position, aktifHiz * Time.deltaTime);
    }

    void YonuKaraktereCevir()
    {
        if (player.position.x > transform.position.x) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;
    }

    void EtrafiKolacanEt()
    {
        aramaZamanlayicisi -= Time.deltaTime;
        if (aramaZamanlayicisi <= 0)
        {
            bool rastgeleYon = (Random.Range(0, 2) == 0);
            spriteRenderer.flipX = rastgeleYon;
            aramaZamanlayicisi = Random.Range(2f, 5f);
        }
    }

    IEnumerator HeyecanlanVeKos()
    {
        suanKosuyor = true;
        yield return new WaitForSeconds(2f);
        suanKosuyor = false;
    }
}
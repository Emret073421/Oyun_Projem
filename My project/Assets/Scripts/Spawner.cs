using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject inekPrefabi;  // Ýnek Kalýbýn (Bull)
    public int inekSayisi = 10;     // Kaç inek olsun?

    [Header("Harita Sýnýrlarý")]
    public Vector2 mapCenter = new Vector2(2.7083f, 3.1969f); // Senin verdiðin merkez
    public Vector2 mapSize = new Vector2(10f, 10f); // Haritanýn Geniþliði ve Yüksekliði

    [Header("Güvenli Bölge")]
    public float karaktereUzaklik = 3f; // Karakterin ne kadar yakýnýna doðmasýn?
    public Transform player; // Karakterin kendisi

    void Start()
    {
        // Eðer karakteri sürüklemeyi unuttuysan otomatik bulsun
        if (player == null)
            player = FindObjectOfType<PlayerController>().transform;

        for (int i = 0; i < inekSayisi; i++)
        {
            SpawnCow();
        }
    }

    void SpawnCow()
    {
        Vector2 spawnPos = Vector2.zero;
        bool uygunYerBulundu = false;

        // Uygun yer bulana kadar 10 kere dene (Sonsuz döngüye girmesin diye limit koyduk)
        for (int i = 0; i < 10; i++)
        {
            // 1. Harita sýnýrlarý içinde rastgele bir nokta seç
            float randomX = Random.Range(mapCenter.x - mapSize.x / 2, mapCenter.x + mapSize.x / 2);
            float randomY = Random.Range(mapCenter.y - mapSize.y / 2, mapCenter.y + mapSize.y / 2);

            spawnPos = new Vector2(randomX, randomY);

            // 2. Bu nokta karaktere çok mu yakýn? Kontrol et.
            if (Vector2.Distance(spawnPos, player.position) > karaktereUzaklik)
            {
                uygunYerBulundu = true;
                break; // Bulduk! Döngüden çýk.
            }
        }

        // Eðer uygun yer bulduysak ineði yarat
        if (uygunYerBulundu)
        {
            Instantiate(inekPrefabi, spawnPos, Quaternion.identity);
        }
    }

    // Harita sýnýrlarýný Editörde çizgi olarak görmek için
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        // Harita Sýnýrlarýný Çiz
        Gizmos.DrawWireCube(mapCenter, new Vector3(mapSize.x, mapSize.y, 1));

        if (player != null)
        {
            Gizmos.color = Color.red;
            // Karakterin etrafýndaki güvenli alaný çiz
            Gizmos.DrawWireSphere(player.position, karaktereUzaklik);
        }
    }
}
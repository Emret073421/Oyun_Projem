using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Karakterin buraya gelecek

    void LateUpdate()
    {
        if (target == null) return;

        // Kameranýn Z'si (-10) bozulmasýn, sadece X ve Y karakteri takip etsin
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
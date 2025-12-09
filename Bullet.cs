using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    void Update()
    {
        if (GalagaManager.Instance == null || !GalagaManager.Instance.running) return;
        transform.position += Vector3.up * speed * Time.deltaTime;
        if (transform.position.y > GalagaManager.Instance.CameraY + GalagaManager.Instance.WorldHeight / 2f)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (GalagaManager.Instance != null) GalagaManager.Instance.HandleCollision(gameObject, collision.gameObject);
    }
}
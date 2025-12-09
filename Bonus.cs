using UnityEngine;
public class Bonus : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (GalagaManager.Instance != null) GalagaManager.Instance.HandleCollision(gameObject, collision.gameObject);
    }
}
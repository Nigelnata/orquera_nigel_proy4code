using UnityEngine;

public class BlockController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
      
        if (collision.gameObject.name.Contains("Ball"))
        {
            
            Arkanoid.score++;

            
            gameObject.SetActive(false);
        }
    }
}
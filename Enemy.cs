using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int type;
    private float speedX = 4f;
    private int shootRate = 100;
    private int shootCooldown;

    private int zigSign = 1;
    private int zigCounter = 0;

    public GameObject enemyBulletPrefab;

    public void Initialize(int enemyType)
    {
        this.type = enemyType;
        shootCooldown = shootRate;
    }

    void Update()
    {
        if (GalagaManager.Instance == null || !GalagaManager.Instance.running) return;

        if (type == 1)
        {
            float newX = transform.position.x + speedX * zigSign * Time.deltaTime;
            zigCounter++;
            if (zigCounter % 60 == 0) zigSign *= -1;

            float worldLimitX = GalagaManager.Instance.WorldWidth / 2f;
            float halfWidth = transform.localScale.x / 2f;

            if (newX < -worldLimitX + halfWidth) { newX = -worldLimitX + halfWidth; zigSign = 1; }
            if (newX > worldLimitX - halfWidth) { newX = worldLimitX - halfWidth; zigSign = -1; }

            transform.position = new Vector3(newX, transform.position.y, 0);
        }

        if (shootCooldown > 0) shootCooldown--;
        if (shootCooldown == 0)
        {
            Vector3 spawnPosition = transform.position + Vector3.down * 0.5f;
            GalagaManager.Instance.InstantiateObject(enemyBulletPrefab, spawnPosition);
            shootCooldown = 60;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (GalagaManager.Instance != null)
        {
            GalagaManager.Instance.HandleCollision(gameObject, collision.gameObject);
        }
    }
}
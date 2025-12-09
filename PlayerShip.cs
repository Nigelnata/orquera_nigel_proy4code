using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public float movementSpeed = 4f;
    public float shootingCooldown = 0.2f;
    private float lastShotTime;

    public GameObject bulletPrefab;

    void Start()
    {
        bulletPrefab = GalagaManager.Instance.PlayerBulletPrefab;
    }

    void Update()
    {
        if (GalagaManager.Instance == null || !GalagaManager.Instance.running) return;

        float horizontalInput = 0;
        float verticalInput = 0;

        if (Input.GetKey(KeyCode.A)) horizontalInput -= 1;
        if (Input.GetKey(KeyCode.D)) horizontalInput += 1;
        if (Input.GetKey(KeyCode.W)) verticalInput += 1;
        if (Input.GetKey(KeyCode.S)) verticalInput -= 1;

        Vector3 move = new Vector3(horizontalInput, verticalInput, 0);
        transform.position += move.normalized * movementSpeed * Time.deltaTime;

        float worldLimitX = GalagaManager.Instance.WorldWidth / 2f;
        float halfWidth = transform.localScale.x / 2f;
        float clampedX = Mathf.Clamp(transform.position.x, -worldLimitX + halfWidth, worldLimitX - halfWidth);
        transform.position = new Vector3(clampedX, transform.position.y, 0);

        if (Input.GetKey(KeyCode.Space) && Time.time > lastShotTime + shootingCooldown)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * 0.5f;
            GalagaManager.Instance.InstantiateObject(bulletPrefab, spawnPosition);
            lastShotTime = Time.time;
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
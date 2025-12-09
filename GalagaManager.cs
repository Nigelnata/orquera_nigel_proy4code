using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GalagaManager : MonoBehaviour
{
    public static GalagaManager Instance;

    public Transform PlayerShipTransform;
    public TextMeshProUGUI puntajeText;
    public TextMeshProUGUI vidasText;
    public TextMeshProUGUI gameoverText;
    public GameObject PlayerBulletPrefab;
    public GameObject EnemyPrefab;
    public GameObject EnemyBulletPrefab;
    public GameObject BonusPrefab;
    public GameObject GoalPrefab;
    public GameObject StarPrefab;

    public float WorldWidth = 40f;
    public float WorldHeight = 40f;

    public float CamSpeed = 3f;
    public float CamAccel = 0.004f;
    public float CameraY = 0f;

    public float playerSpeed = 4f;

    private float spawnTimer = 0.5f;
    private int contador = 0;
    public float spawnRate = 1.66f;
    private float timeSinceLastSpawn;

    public static int score;
    public static int vidas;
    public bool gameover;
    public bool running;
    public bool won;
    bool loop = true;

    private void ponerpos(Transform obj, float x, float y)
    {
        obj.position = new Vector3(x, y, 0);
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        gameoverText.gameObject.SetActive(false);
        gamereset();
    }

    void Update()
    {
        ProcessInput();
        UpdateGameLogic();
        UpdateSpawner();
        PaintScreen();
    }

    void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            loop = false;
        }

        if (gameover && Input.GetKey(KeyCode.R))
        {
            gamereset();
        }
    }

    void UpdateGameLogic()
    {
        if (!running) return;

        CameraY += CamSpeed * Time.deltaTime;
        CamSpeed += CamAccel * Time.deltaTime;
        Camera.main.transform.position = new Vector3(0, CameraY, -10);

        float cameraBottom = CameraY - WorldHeight / 2f;

        if (PlayerShipTransform.position.y < cameraBottom - 5f)
        {
            vidas--;
            ponerpos(PlayerShipTransform, 0, CameraY + WorldHeight / 2f - 5f);
        }

        if (vidas <= 0)
        {
            running = false;
            gameover = true;
            vidas = 0;
        }
    }

    void UpdateSpawner()
    {
        if (!running) return;

        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
            return;
        }

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnRate)
        {
            float[] posicionesX = { -15f, -10f, -5f, 0f, 5f, 10f };
            int[] tipos = { 0, 1, 0, 1, 0, 1 };

            float spawnY = CameraY + WorldHeight / 2f + 5f;
            float spawnX = posicionesX[contador % posicionesX.Length];
            int type = tipos[contador % tipos.Length];

            Color c;
            if (type == 0) c = new Color(0.89f, 0.45f, 0.65f);
            else c = Color.green;

            GameObject newEnemy = InstantiateObject(EnemyPrefab, new Vector3(spawnX, spawnY, 0));
            newEnemy.GetComponent<Enemy>().Initialize(type);
            newEnemy.GetComponent<Enemy>().enemyBulletPrefab = EnemyBulletPrefab;
            newEnemy.GetComponent<SpriteRenderer>().color = c;

            contador++;
            timeSinceLastSpawn = 0f;
        }
    }

    public void HandleCollision(GameObject obj1, GameObject obj2)
    {
        if (!running) return;

        string name1 = obj1.GetComponent<MonoBehaviour>().GetType().Name;
        string name2 = obj2.GetComponent<MonoBehaviour>().GetType().Name;

        if ((name1 == "Bullet" && name2 == "Enemy") || (name1 == "Enemy" && name2 == "Bullet"))
        {
            GameObject bullet = (name1 == "Bullet") ? obj1 : obj2;
            GameObject enemy = (name1 == "Enemy") ? obj1 : obj2;

            Destroy(enemy);
            Destroy(bullet);
            score += 100;
        }

        else if (name1 == "PlayerShip" && (name2 == "Enemy" || name2 == "EnemyBullet"))
        {
            Destroy(obj2);
            vidas--;
        }
        else if (name2 == "PlayerShip" && (name1 == "Enemy" || name1 == "EnemyBullet"))
        {
            Destroy(obj1);
            vidas--;
        }

        else if ((name1 == "PlayerShip" && name2 == "Bonus") || (name1 == "Bonus" && name2 == "PlayerShip"))
        {
            GameObject bonus = (name1 == "Bonus") ? obj1 : obj2;
            Destroy(bonus);
            vidas++;
            score += 50;
        }

        else if ((name1 == "PlayerShip" && name2 == "Goal") || (name1 == "Goal" && name2 == "PlayerShip"))
        {
            won = true;
            running = false;
            gameover = true;
        }
    }

    void PaintScreen()
    {
        puntajeText.text = "puntaje: " + score;
        vidasText.text = "vidas: " + vidas;

        if (gameover)
        {
            if (won)
                gameoverText.text = "¡has ganado! presione r para reiniciar";
            else
                gameoverText.text = "ha perdido, presione r para reiniciar";

            gameoverText.gameObject.SetActive(true);
        }
        else
        {
            gameoverText.gameObject.SetActive(false);
        }
    }

    protected void gamereset()
    {
        foreach (var obj in FindObjectsOfType<MonoBehaviour>())
        {
            string name = obj.GetType().Name;
            
            if (name == "PlayerShip" || name == "Enemy" || name == "Bullet" || name == "EnemyBullet" || name == "Bonus" || name == "Goal" || name == "Star")
            {
                if (obj.gameObject != gameObject && obj.gameObject != PlayerShipTransform.gameObject)
                {
                    Destroy(obj.gameObject);
                }
            }
        }

        running = true;
        won = false;
        gameover = false;
        CamSpeed = 3f;
        CameraY = 0f;

        score = 0;
        vidas = 3;

        ponerpos(PlayerShipTransform, 0, -WorldHeight / 2f + 5f);

        contador = 0;
        spawnTimer = 0.5f;
        Camera.main.orthographicSize = WorldHeight / 2f;

        for (float x = -WorldWidth / 2f + 1f; x < WorldWidth / 2f; x += 5f)
        {
            for (float y = -WorldHeight / 2f; y < WorldHeight / 2f; y += 5f)
            {
                InstantiateObject(StarPrefab, new Vector3(x, y, 10));
            }
        }

        InstantiateObject(BonusPrefab, new Vector3(-15f, 20f, 0));
        InstantiateObject(BonusPrefab, new Vector3(-5f, 5f, 0));
        InstantiateObject(BonusPrefab, new Vector3(10f, -10f, 0));
        InstantiateObject(GoalPrefab, new Vector3(0, 100f, 0));
    }

    public GameObject InstantiateObject(GameObject prefab, Vector3 position)
    {
        return Instantiate(prefab, position, Quaternion.identity);
    }
}
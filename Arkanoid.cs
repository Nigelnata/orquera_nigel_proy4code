using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Arkanoid : MonoBehaviour
{
    public Transform PlayerPaddle;
    public Transform Ball;
    public TextMeshProUGUI puntajeText;
    public TextMeshProUGUI vidasText;
    public TextMeshProUGUI gameoverText;
    public GameObject BlockPrefab;

    float anchoJuego = 40f;
    float altoJuego = 30f;

    public float playerx;
    public float playery;
    public int sizeX;
    public int sizeY;

    public float ballX;
    public float ballY;
    public int ballWidth;
    public int ballHeight;
    public int ballDirectionX;
    public int ballDirectionY;

    GameObject[] blocks;
    public int[] blockX;
    public int[] blockY;
    public bool[] blockactivo;
    public int blockWidth;
    public int blockHeight;

   
    public static int score;
    public static int vidas;

    public bool gameover;
    bool loop = true;

    private void ponerpos(Transform obj, float x, float y)
    {
        obj.position = new Vector3(x, y, 0);
    }

    private void escalar(Transform obj, int x, int y)
    {
        obj.localScale = new Vector3(x, y, 1);
    }
    // Start is called before the first frame update

    void Start()
    {
        sizeX = 8;
        sizeY = 2;
        ballWidth = 2;
        ballHeight = 2;
        blockWidth = 40;
        blockHeight = 20;

        escalar(PlayerPaddle, sizeX, sizeY);
        escalar(Ball, ballWidth, ballHeight);

        gameoverText.gameObject.SetActive(false);

        gamereset();
    }

    // Update is called once per frame

    void Update()
    {
        ProcessInput();
        UpdateGameLogic();
        PaintScreen();
    }

    void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            loop = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerx -= 25 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerx += 25 * Time.deltaTime;
        }

        if (gameover && Input.GetKey(KeyCode.R))
        {
            gamereset();
        }

        if (score == 22)
        {
            gamereset();
        }
    }

    void UpdateGameLogic()
    {
        if (gameover)
        {
            return;
        }

        ballX = Ball.position.x;
        ballY = Ball.position.y;

        float limiteX = anchoJuego / 2f - PlayerPaddle.localScale.x / 2f;

        if (playerx < -limiteX)
            playerx = -limiteX;

        if (playerx > limiteX)
            playerx = limiteX;

        float bordeInf = -altoJuego / 2f - Ball.localScale.y / 2f;

        if (ballY < bordeInf)
        {
            vidas--;
            Debug.Log("el jugador ha perdido una vida");

            var rb = Ball.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;

            if (vidas > 0)
            {
                Ball.position = new Vector3(0, 2f, 0);
                rb.velocity = new Vector2(18 * ballDirectionX, 18 * ballDirectionY);
            }

            if (vidas <= 0)
            {
                gameover = true;
            }
        }

        ponerpos(PlayerPaddle, playerx, playery);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.name == "PlayerPaddle")
        {
        }
    }

    void PaintScreen()
    {
        puntajeText.text = "puntaje " + score;
        vidasText.text = "vidas " + vidas;

        if (gameover)
        {
            gameoverText.text = "ha perdido, presione R para reiniciar";
            gameoverText.gameObject.SetActive(true);
        }
        else
        {
            gameoverText.gameObject.SetActive(false);
        }
    }

    protected void gamereset()
    {
        if (blocks != null)
        {
            foreach (var block in blocks)
            {
                if (block != null)
                {
                    Destroy(block);
                }
            }
        }

        playerx = 0;
        playery = -14f;

        ballX = 0;
        ballY = 0;
        ballDirectionX = 1;
        ballDirectionY = 1;

        blockX = new int[40];
        blockY = new int[40];
        blockactivo = new bool[40];
        blocks = new GameObject[40];

        score = 0;
        vidas = 3;
        gameover = false;

        int contblock = 0;
        float widthUnit = blockWidth / 10f;
        float heightUnit = blockHeight / 10f;
        float spaceX = 5.1f;

       
        System.Action<float, float, Color> CreateBlock = (x, y, color) =>
        {
            var newblock = Instantiate(BlockPrefab, new Vector3(x, y, 0), Quaternion.identity);

           
            var rbBlock = newblock.AddComponent<Rigidbody2D>();
            rbBlock.bodyType = RigidbodyType2D.Static; 
           

            newblock.transform.localScale = new Vector3(widthUnit, heightUnit, 1);
            newblock.GetComponent<SpriteRenderer>().color = color;
            newblock.name = "BlockP " + contblock;
            blocks[contblock] = newblock;
            blockactivo[contblock] = true;
            contblock++;
        };

        
        float startX1 = -15.5f;
        float yPos1 = 11.25f;
        for (int i = 0; i < 7; i++)
        {
            CreateBlock(startX1 + i * spaceX, yPos1, Color.magenta);
        }

       
        float startX2 = -13.5f;
        float yPos2 = 9f;
        for (int i = 0; i < 6; i++)
        {
            CreateBlock(startX2 + i * spaceX, yPos2, new Color(0.86f, 0.33f, 0.5f));
        }

       
        float startX3 = -11.5f;
        float yPos3 = 6.75f;
        for (int i = 0; i < 6; i++)
        {
            CreateBlock(startX3 + i * spaceX, yPos3, new Color(0.98f, 0.62f, 0.79f));
        }

      
        float startX4 = -9.5f;
        float yPos4 = 4.5f;
        for (int i = 0; i < 4; i++)
        {
            CreateBlock(startX4 + i * spaceX, yPos4, new Color(1.0f, 0.41f, 0.71f));
        }

        var rb = Ball.GetComponent<Rigidbody2D>();
        Ball.position = new Vector3(0, 2f, 0);
        rb.velocity = new Vector2(18 * ballDirectionX, 18 * ballDirectionY);

        Debug.Log("Juego reiniciado");
    }
}
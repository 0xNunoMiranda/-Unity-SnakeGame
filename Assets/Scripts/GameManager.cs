using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum Direction
    {
        LEFT, UP, DOWN, RIGHT

    }

    public GameObject PanelGameOver;
    public GameObject PanelTitle;
    public Direction moveDirection; //armazena a direcao

    public Text txtScore;

    public Text txtHiScore;

    private int score = 0;

    private int hiscore = 0;
    public float delayStep; // tempo entre um passo e outro

    public float step; //quantidade de movimentos a cada passo, espaço da grelha dos blocos

    public Transform HeadPlayer;

    public List<Transform> Tail;

    private Vector3 lastPos; //ultima posicao do snake

    public GameObject foodPrefab;
    public GameObject tailPrefab;

    public int collums;

    public int rows;

    public Animator gameOverAnimation;

    void Awake()
    {

        PanelTitle.SetActive(true);


    }

    // Start is called before the first frame update

    void Start()
    {


        setFood();

        hiscore = PlayerPrefs.GetInt("HiScore");

        txtHiScore.text = " HiScore: " + hiscore;


    }

    // Update is called once per frame
    void Update()
    {


        if (PanelTitle.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {

            Play();
        }
        if (PanelGameOver.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            Play();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            moveDirection = Direction.UP;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDirection = Direction.LEFT;

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveDirection = Direction.DOWN;

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDirection = Direction.RIGHT;

        }
    }

    IEnumerator MoveSnake()
    {
        yield return new WaitForSeconds(delayStep);
        Vector3 nexPosition = Vector3.zero;
        switch (moveDirection)
        {

            case Direction.DOWN:
                nexPosition = Vector3.down;
                HeadPlayer.rotation = Quaternion.Euler(0, 0, 90);

                break;

            case Direction.UP:
                nexPosition = Vector3.up;
                HeadPlayer.rotation = Quaternion.Euler(0, 0, -90);

                break;

            case Direction.LEFT:
                nexPosition = Vector3.left;
                HeadPlayer.rotation = Quaternion.Euler(0, 0, 0);

                break;

            case Direction.RIGHT:
                nexPosition = Vector3.right;
                HeadPlayer.rotation = Quaternion.Euler(0, 0, 180);

                break;

        }
        nexPosition *= step;
        lastPos = HeadPlayer.position; //posição atual, que será vista como a anterior
        HeadPlayer.position += nexPosition; // nova posicao recebida

        // Atualização das novas posições da cauda
        foreach (Transform t in Tail)
        {
            Vector3 temp = t.position;
            if (HeadPlayer.position == t.position)
            {
                setHiscore();
            }
            t.position = lastPos;
            lastPos = temp;
            t.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        StartCoroutine("MoveSnake");
    }

    public void Eat()
    {
        setFood();
        score += 10;
        txtScore.text = "Score: " + score + " ";
        Vector2 tailPostion = HeadPlayer.position;
        if (Tail.Count > 0)
        {
            tailPostion = Tail[Tail.Count - 1].position;

        }

        GameObject temp = Instantiate(tailPrefab, tailPostion, transform.localRotation);
        Tail.Add(temp.transform);

    }

    void setFood()
    {
        int x = Random.Range((collums / 2) * -1, (collums / 2));
        int y = Random.Range((rows / 2) * -1, (rows / 2));

        foodPrefab.GetComponent<Transform>().position = new Vector2(x * step, y * step);

    }

    public void setHiscore()
    {
        StopAllCoroutines();
        
        if (score > hiscore)
        {
            PlayerPrefs.SetInt("HiScore", score);
        };

        PanelGameOver.SetActive(true);

    }

    public void Play()
    {

        StartCoroutine("MoveSnake");
        PanelGameOver.SetActive(false);
        PanelTitle.SetActive(false);
        HeadPlayer.position = Vector3.zero;
        score = 0;
        txtScore.text = "Score: " + score + " ";
        moveDirection = Direction.LEFT;
        Time.timeScale = 1;
        foreach (Transform t in Tail)
        {
            Destroy(t.gameObject);
        }
        Tail.Clear();
    }


}

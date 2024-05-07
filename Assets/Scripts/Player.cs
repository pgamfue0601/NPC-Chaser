using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float velocidadMovimiento = 15f;
    public float velocidadRotacion = 100f;
    public float health = 100f;
    public float velocidadInicial, velocidadSprint;
    private int booksFound;
    private int booksToFind = 1;
    private GameManager gameManager;
    public float x, y;
    [SerializeField] private TextMeshProUGUI currentBookText;
    [SerializeField] private TextMeshProUGUI bookToFindText;
    private Animator anim;

    public Camera camBack;

    public Rigidbody rb;
    public bool winner;
    public bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        camBack.enabled = true;
        velocidadInicial = velocidadMovimiento * 15f * Time.fixedDeltaTime;
        velocidadSprint = velocidadMovimiento * 40f * Time.fixedDeltaTime;

        if (GameObject.Find("GameManager"))
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            booksToFind = gameManager.instance.booksToBeFound;
        }

        bookToFindText.text = booksToFind.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            return;
        }
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (booksFound >= booksToFind)
        {
            winner = true;
            SceneManager.LoadScene("WinnerScene");
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("sprint", true);
            velocidadMovimiento = velocidadSprint;

        }
        else
        {
            anim.SetBool("sprint", false);
            velocidadMovimiento = velocidadInicial;

        }

        if (health <= 0)
        {
            StartCoroutine(EnemyTouched());
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, x * Time.fixedDeltaTime * velocidadRotacion, 0);
        transform.Translate(0, 0, y * Time.fixedDeltaTime * velocidadMovimiento);

        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);
    }

    public void BookFound()
    {
        booksFound++;
        currentBookText.text = booksFound.ToString();
    }

    public IEnumerator EnemyTouched()
    {
        dead = true;
        anim.SetTrigger("death");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void PlayerDamaged(int damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
    }
}

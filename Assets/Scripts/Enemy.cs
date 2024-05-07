using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector3 min, max;
    [SerializeField] private Vector3 destination;
    [SerializeField] private bool playerDetected = false;
    [SerializeField] private bool playerAttack = false;
    private Animator anim;
    private GameManager gameManager;
    private GameObject player;
    [SerializeField] private float playerDistanceDetection = 30;
    [SerializeField] private float playerAttackDistance = 30;
    [SerializeField] private float visionAngle = 45;
    private int damage = 10;
    public float distance;
    public float playerDistance;

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");

        if (GameObject.Find("GameManager"))
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            switch (gameManager.instance.difficulty)
            {
                case 0:
                    GetComponent<NavMeshAgent>().speed = 5.5f;
                    damage = 15;
                    break;
                case 1:
                    GetComponent<NavMeshAgent>().speed = 7.5f;
                    damage = 35;
                    break;
                case 2:
                    GetComponent<NavMeshAgent>().speed = 8.5f;
                    damage = 65;
                    break;
                default:
                    return;

            }
        }
        RandomDestination();
        anim = GetComponent<Animator>();
        //Iniciamos Corrutina
        StartCoroutine(Patroll());
        StartCoroutine(Alert());

    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, destination);
        playerDistance = Vector3.Distance(transform.position, player.transform.position);
        //if (playerDetected)
        //{
        //    destination = player.transform.position;
        //}
        if (distance < 1.5f)
        {
            StartCoroutine(Patroll());
        }

        if (Vector3.Distance(transform.position, player.transform.position) < playerDistanceDetection) //si la distancia es menor de metro y medio
        {
             StartCoroutine(Alert());
        }

        if (Vector3.Distance(transform.position, player.transform.position) < playerAttackDistance)
        {
            StartCoroutine(Attack());
        }

        if (Vector3.Distance(transform.position, player.transform.position) < playerDistanceDetection)
        {
            StopAllCoroutines();
            StartCoroutine(Patroll());
            StartCoroutine(Alert());
        }

        anim.SetFloat("speed",GetComponent<NavMeshAgent>().velocity.magnitude);
    }

    private void RandomDestination()
    {
        destination = new Vector3(Random.Range(min.x, max.x), 1, Random.Range(max.z, min.z));
        GetComponent<NavMeshAgent>().SetDestination(destination);
    }
    IEnumerator Patroll()
    {
            if (Vector3.Distance(transform.position, destination) < 1.5f) //si la distancia es menor de metro y medio
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 3f));
                RandomDestination();
            }
            yield return new WaitForEndOfFrame();
    }

    IEnumerator Alert()
    {
            if (Vector3.Distance(transform.position, player.transform.position) < playerDistanceDetection) //si la distancia es menor de metro y medio
            {
                transform.LookAt(player.transform.position);
                Vector3 vectorPlayer = player.transform.position - transform.position;
                if (Vector3.Angle(vectorPlayer.normalized, transform.forward) < visionAngle)
                {
                    playerDetected = true;
                    GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
                    StopCoroutine(Patroll());
            }
                else
                {
                    playerDetected = false;
                }

            }
            yield return new WaitForEndOfFrame();
        
    }

    IEnumerator Attack()
    {
        StopCoroutine("Alert");
            transform.LookAt(player.transform.position);
            if (Vector3.Distance(transform.position, player.transform.position) <= playerAttackDistance)
            {
                //GetComponent<NavMeshAgent>().isStopped = true;
                destination = player.transform.position;
                GetComponent<NavMeshAgent>().SetDestination(destination);

                //Atacamos
            playerAttack = true;
            }
            else
            {
                GetComponent<NavMeshAgent>().SetDestination(transform.position);
                GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                
                playerAttack = false;
                if (Vector3.Distance(transform.position, player.transform.position) > playerDistanceDetection)
                {
                    playerDetected = false;
                    StartCoroutine(Patroll());
                    StartCoroutine(Alert());
                    StopCoroutine(Attack());
                    //break;
                }


                }
            yield return new WaitForEndOfFrame();
    }


    //Detección por trigger
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<Player>().dead)
            {
                Debug.Log("Muerto");
                collision.gameObject.GetComponent<Player>().PlayerDamaged(damage);
            }
            
        }
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerDetected = false;
            StartCoroutine(Patroll());
        }
    }*/
}

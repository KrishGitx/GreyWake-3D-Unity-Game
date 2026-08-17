using System;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SpiderAI : MonoBehaviour
{

    public float Health = 100;

    NavMeshAgent agent;
    Animator animController;

    public enum EnemyState
    {
        Idle,
        Moving,
        Chase
    }
    public EnemyState CurrentState;

    //For Random Positioning
    Vector3 initialPos;
    [SerializeField] float maxDistance;

    // To check if idle Time passed at TargetPos
    float currIdleTime;
    [SerializeField] float maxIdleTime;

    float currPosTimer;
    [SerializeField] float distanceBtwPlayer;
    Vector3 targetPos;
    Vector3 pos;
    [SerializeField] float rotSpeed;

    public GameObject Player;

    //Attacking:
    [SerializeField] int damage;
    float timeToAttack = 0f;

    AudioSource audioSrc;

    public AudioClip bite;
    public AudioClip death;

    // Start is called once before the first execution of Update after the MonoBehaviour is create
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animController = GetComponent<Animator>();
        CurrentState = EnemyState.Idle;
        initialPos = transform.position;

        audioSrc = gameObject.GetComponent<AudioSource>();

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (Health <= 0f)
        {
            animController.SetTrigger("isDead");
            audioSrc.PlayOneShot(death);
            audioSrc.volume = 0.5f;
            Destroy(gameObject, 1.55f);
        }

        if (currIdleTime >= 0f) currIdleTime -= Time.deltaTime;
        // if(timeToAttack >=0f ) timeToAttack -= Time.deltaTime;
        if (CurrentState == EnemyState.Idle)
        {
            animController.SetBool("isMoving", true);
            Vector3 randomPoint;
            NavMeshHit hit;
            bool found = false;

            for (int i = 0; i < 10; i++)
            {
                float x = UnityEngine.Random.Range(initialPos.x - maxDistance, initialPos.x + maxDistance);
                float z = UnityEngine.Random.Range(initialPos.z - maxDistance, initialPos.z + maxDistance);

                randomPoint = new Vector3(x, transform.position.y, z);

                if (NavMesh.SamplePosition(randomPoint, out hit, 3f, NavMesh.AllAreas))
                {
                    targetPos = hit.position;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                agent.SetDestination(targetPos);
            }

            currPosTimer = 2f;
            agent.updatePosition = true;
            agent.updateRotation = true;
            agent.isStopped = false;

            pos = transform.position;
            NavMeshHit hitx;

            if (NavMesh.SamplePosition(targetPos, out hitx, 2f, NavMesh.AllAreas))
            {
                agent.SetDestination(hitx.position);
            }
            else
            {
                Debug.Log("Couldn't find a point on the NavMesh.");
            }

            currIdleTime = maxIdleTime;
            CurrentState = EnemyState.Moving;


        }
        else if (CurrentState == EnemyState.Moving)
        {

            if (currIdleTime <= 0f)
            {
                agent.updatePosition = true;
                agent.updateRotation = true;
                agent.isStopped = false;
                animController.SetBool("isMoving", true);
                if (Vector3.Distance(transform.position, targetPos) < 2f)
                {
                    CurrentState = EnemyState.Idle;
                    currIdleTime = maxIdleTime;
                    agent.ResetPath();
                    agent.updatePosition = false;
                    agent.updateRotation = false;
                    agent.isStopped = true;
                }
            }
            else
            {
                agent.updatePosition = false;
                agent.updateRotation = false;
                agent.isStopped = true;
                animController.SetBool("isMoving", false);

            }
        }
        else if (CurrentState == EnemyState.Chase)
        {
            agent.updateRotation = false;
            animController.SetBool("isMoving", true);
            Quaternion targetRot = Quaternion.LookRotation(Player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, Player.transform.position) > distanceBtwPlayer)
            {
                agent.SetDestination(Player.transform.position);
            }
            else if (Vector3.Distance(transform.position, Player.transform.position) < distanceBtwPlayer)
            {
                agent.ResetPath();
                animController.SetBool("isMoving", false);

                if (timeToAttack <= 0f)
                {
                    animController.SetTrigger("Attack");
                    Player.GetComponent<PlayerManager>().Health -= damage;
                    timeToAttack = 1.2f;
                }
                else
                {
                    timeToAttack -= Time.deltaTime;
                }
            }
            if (Vector3.Distance(transform.position, Player.transform.position) > 13f) { CurrentState = EnemyState.Idle; animController.SetBool("isMoving", false); }
        }

        checkEnemyMovement();
    }

    public void biteSound()
    {
        audioSrc.PlayOneShot(bite);
    }

    void checkEnemyMovement()
    {

        if (currPosTimer > 0f)
        {
            currPosTimer -= Time.deltaTime;
        }

        if (currPosTimer <= 0f && currPosTimer != -1)
        {
            if (!agent.pathPending &&
                agent.hasPath &&
                agent.velocity.magnitude < 0.1f)
            {
                Debug.Log("Agent is stuck or not moving");
            }
            currPosTimer = -1;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            agent.ResetPath();
            CurrentState = EnemyState.Chase;
        }
        else if (other.tag == "autoOpen")
        {
            if (Vector3.Distance(transform.position, other.transform.position) < 2.5f)
            {
                Interaction sc = other.GetComponentInParent<Interaction>();
                if (sc.canInteract) sc.Door(false, 4f);
            }
        }
    }

}


using System;
using Unity.VisualScripting;
using UnityEngine;

public class DroneAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject[] dronePoitns;
    public enum EnemyState
    {
        Idle,
        Moving
    }
    public EnemyState currState;

    Vector3 targetPos;


    int rots;
    float idleTime;
    float rotationValue;
    bool rotated;
    bool targetSet;
    Quaternion targetRotation;

    [SerializeField] float rotSpeed;
    [SerializeField] float speed;
    [SerializeField] float maxIdleTime;


    void Start()
    {
        currState = EnemyState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (idleTime > 0f) idleTime -= Time.deltaTime;
        if (currState == EnemyState.Idle)
        {
            targetSet = false;

            rotated = false;
            int ran = UnityEngine.Random.Range(0, dronePoitns.Length - 1);
            targetPos = dronePoitns[ran].transform.position;

            if (!targetSet)
            {
                Vector3 direction = dronePoitns[ran].transform.position - transform.position;
                direction.y = 0f;

                targetRotation = Quaternion.LookRotation(direction);

                targetSet = true;
            }

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f) currState = EnemyState.Moving;
        }
        else if (currState == EnemyState.Moving)
        {
            if (Vector3.Distance(transform.position, targetPos) > 1f) MoveDrone();
            else
            {
                int len = 2;
                if (rots <= len)
                {
                    if (idleTime <= 0f) idleDrone();
                }
                else
                {
                    currState = EnemyState.Idle;
                }

            }

        }
    }



    void idleDrone()
    {
        if (!rotated)
        {
            if (!targetSet)
            {
                rotationValue = UnityEngine.Random.Range(30f, 90f);
                targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + rotationValue, 0f);
                targetSet = true;
            }

            if (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
            {
                Debug.Log("FASDFGASDEADF");
                transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotSpeed * Time.deltaTime
            );
            }
            else
            {
                targetSet = false;
                idleTime = maxIdleTime;
                rots++;
                rotated = true;
            }
        }
    }
    void MoveDrone()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}

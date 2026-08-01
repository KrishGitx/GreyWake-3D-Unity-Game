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

    float idleTime;
    float rotSpeed;
    bool rotated;

    [SerializeField] float speed;
    [SerializeField] float maxIdleTime;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (idleTime > 0f) idleTime -= Time.deltaTime;
        if (currState == EnemyState.Idle)
        {
            int ran = UnityEngine.Random.Range(0, dronePoitns.Length - 1);

            targetPos = dronePoitns[ran].transform.position;
            currState = EnemyState.Moving;
        }
        else if (currState == EnemyState.Moving)
        {
            if (Vector3.Distance(transform.position, targetPos) > 1f) MoveDrone();
            else
            {
                idleTime = maxIdleTime;
                Invoke("idleDrone", 3f);
            }

        }
    }



    void idleDrone()
    {
        if (!rotated)
        {
            float rotationValue = UnityEngine.Random.Range(30f, 90f);

            if (transform.eulerAngles.y <= rotationValue)
            {
                transform.Rotate(0f, rotSpeed * Time.deltaTime, 0f);
            }
            else
            {
                rotated = true;
            }
        }
    }
    void MoveDrone()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}

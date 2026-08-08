using UnityEngine;

public class DroneAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject[] dronePoints;
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


    LineRenderer line;

    int len = 2;
    void Start()
    {
        currState = EnemyState.Idle;
        line = gameObject.GetComponentInChildren<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 start = line.GetPosition(0);
        Vector3 end = line.GetPosition(1);

        Vector3 lineDir = (end - start).normalized;
        float distance = Vector3.Distance(start, end);

        if (Physics.Raycast(start, lineDir, out RaycastHit hit, distance))
        {
            if(hit.collider.tag == "Player")
            {
                Debug.Log("Restart Game! PlY ded");
            }
        }

        if (idleTime > 0f) idleTime -= Time.deltaTime;
        if (currState == EnemyState.Idle)
        {

            if (!targetSet)
            {
                rotated = false;
                rots = 0;
                int ran = UnityEngine.Random.Range(0, dronePoints.Length);
                targetPos = dronePoints[ran].transform.position;

                Vector3 direction = dronePoints[ran].transform.position - transform.position;
                direction.y = 0f;

                float targetY = Quaternion.LookRotation(direction).eulerAngles.y;

                targetRotation = Quaternion.Euler(
                    transform.eulerAngles.x,
                    targetY + 90f,
                    transform.eulerAngles.z
                );

                targetSet = true;
            }

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                targetSet = false;
                currState = EnemyState.Moving;
            }
        }
        else if (currState == EnemyState.Moving)
        {
            if (Vector3.Distance(transform.position, targetPos) > 1f) MoveDrone();
            else
            {
                if (rots <= len)
                {
                    if (idleTime <= 0f)
                    {
                        rotated = false;
                        idleDrone();
                    }
                }
                else
                {
                    targetSet = false;
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
                targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + rotationValue, transform.eulerAngles.z);
                targetSet = true;
            }

            if (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
            {
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

using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] Transform topPoint;
    [SerializeField] Transform basePoint;

    [SerializeField] Transform baseElevator;

    bool isDown = true;
    bool navPickedUp = true;
    bool Triggered;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Triggered)
        {
            if (navPickedUp && isDown)
            {
                if (Vector3.Distance(baseElevator.transform.position, topPoint.transform.position) > 1f)
                {
                    baseElevator.transform.position = Vector3.MoveTowards(baseElevator.transform.position, new Vector3(baseElevator.transform.position.x, topPoint.position.y, baseElevator.transform.position.z), speed * Time.deltaTime);
                }
            }
            else if (!isDown)
            {
                if (Vector3.Distance(baseElevator.transform.position, basePoint.transform.position) > 1f)
                {
                    baseElevator.transform.position = Vector3.MoveTowards(baseElevator.transform.position, new Vector3(transform.position.x, basePoint.position.y, baseElevator.transform.position.z), speed * Time.deltaTime);
                }
                else
                {
                    isDown = true;
                    Triggered = false;
                }
            }
            else if (!navPickedUp && isDown)
            {
                Debug.Log("Cant Leave yet");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Triggered = true;
        }
    }

}

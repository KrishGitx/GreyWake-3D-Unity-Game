using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxisRaw("Mouse X");
        float yAxis = Input.GetAxisRaw("Mouse Y");

        // float yaw += xAxis;
        // float pitch -= yAxis;

        
     }
}

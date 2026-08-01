
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float speed = 5f;

    Rigidbody rb;

    public AudioClip footstepsClip;
    AudioSource audioSrc;

    bool isWalking;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSrc = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        float xVal = Input.GetAxisRaw("Horizontal");
        float zVal = Input.GetAxisRaw("Vertical");

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();



        Vector3 moveDir = (camForward * zVal + camRight * xVal).normalized;

        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveDir.x * speed;
        velocity.z = moveDir.z * speed;

        rb.linearVelocity = velocity;

    }

    [SerializeField] float stepInterval = 0.45f;
    float stepTimer;

    void Update()
    {
        bool isWalking = rb.linearVelocity.magnitude > 0.1f;

        if (isWalking)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                audioSrc.PlayOneShot(footstepsClip);
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

}

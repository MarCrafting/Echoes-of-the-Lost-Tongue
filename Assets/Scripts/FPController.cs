using UnityEngine;

public class FPController : MonoBehaviour
{
    private CharacterController controller;
    public float moveSpeed = 5f;       // Movement speed
    public float jumpHeight = 2f;       // Jump height
    public float gravity = -9.81f;      // Gravity strength
    private Vector3 velocity;           // For gravity and jumping

    public Transform cameraTransform;   // Reference to the camera
    public float mouseSensitivity = 100f; // Mouse look speed
    private float xRotation = 0f;       // To track vertical rotation

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform not assigned! Please assign the MainCamera.");
        }
        Cursor.lockState = CursorLockMode.Locked; // Lock and hide cursor
    }

    void Update()
    {
        // Movement
        float x = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S
        Vector3 move = transform.right * x + transform.forward * z;
        move = move.normalized * moveSpeed * Time.deltaTime;

        // Gravity and Jumping
        if (controller.isGrounded)
        {
            velocity.y = -2f; // Small downward force to stay grounded
            if (Input.GetKeyDown(KeyCode.Space)) // Jump
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(move + velocity * Time.deltaTime);

        // Mouse Look
        if (cameraTransform != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical look
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX); // Rotate player horizontally
        }
    }
}
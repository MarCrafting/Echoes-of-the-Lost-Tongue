using UnityEngine;

public class FPSController : MonoBehaviour
{
    private CharacterController controller;
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    private Vector3 velocity;

    public Transform cameraTransform;   // MainCamera
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private float yRotation = 0f;       // New for third-person Y-axis rotation

    // View mode and transition
    private bool isFirstPerson = true;
    private bool isTransitioning = false;
    private bool isStartingSkyView = true;
    private float transitionDuration = 2f;
    private float transitionTime = 0f;
    private Vector3 firstPersonPos = new Vector3(0f, 0.95f, 0f);
    private Vector3 thirdPersonPos = new Vector3(0f, 1.5f, -3f);
    private Quaternion skyRotation = Quaternion.Euler(-90f, 0f, 0f);
    private Quaternion thirdPersonRot = Quaternion.Euler(20f, 0f, 0f);

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform not assigned!");
        }
        Cursor.lockState = CursorLockMode.Locked;

        // Start locked looking at sky once game boots
        cameraTransform.localPosition = firstPersonPos;
        cameraTransform.localRotation = skyRotation;
        isFirstPerson = true;
        isStartingSkyView = true;

        // Initialize third-person rotation
        yRotation = transform.eulerAngles.y; // Match player's initial Y rotation
        xRotation = 20f; // Match thirdPersonRot's initial X angle
    }

    void Update()
    {
        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        move = move.normalized * moveSpeed * Time.deltaTime;

        // Gravity and Jumping
        if (controller.isGrounded)
        {
            velocity.y = -2f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(move + velocity * Time.deltaTime);

        // Mouse Look
        if (!isTransitioning && !isStartingSkyView && cameraTransform != null)
        {
            if (isFirstPerson)
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                transform.Rotate(Vector3.up * mouseX);
            }
            else
            {
                // Third-person mouse look
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                yRotation += mouseX; // Horizontal orbit
                xRotation -= mouseY; // Vertical orbit
                xRotation = Mathf.Clamp(xRotation, -10f, 80f); // Limit vertical angle

                UpdateThirdPersonCamera();
            }
        }

        // Force sky view
        if (isStartingSkyView && cameraTransform != null)
        {
            cameraTransform.localRotation = skyRotation;
        }

        // Transition first person to third person camera views
        if (isTransitioning)
        {
            transitionTime += Time.deltaTime;
            float t = transitionTime / transitionDuration;
            if (t >= 1f)
            {
                t = 1f;
                isTransitioning = false;
            }
            cameraTransform.localPosition = Vector3.Lerp(firstPersonPos, thirdPersonPos, t);
            cameraTransform.localRotation = Quaternion.Slerp(skyRotation, thirdPersonRot, t);
        }

        // Test trigger - will replace with new game / continue game buttons logic rather than 'N' key press
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartTransitionToThirdPerson();
        }
    }

    void UpdateThirdPersonCamera()
    {
        // Calculate rotation and position for third-person orbit
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        Vector3 direction = rotation * Vector3.forward;
        cameraTransform.position = transform.position + rotation * (thirdPersonPos.z * Vector3.forward + thirdPersonPos.y * Vector3.up);
        cameraTransform.LookAt(transform.position + Vector3.up * 0.95f); // Look at player's head height
    }

    public void StartTransitionToThirdPerson()
    {
        if (!isTransitioning)
        {
            isStartingSkyView = false;
            isTransitioning = true;
            transitionTime = 0f;
            isFirstPerson = false;
        }
    }
}
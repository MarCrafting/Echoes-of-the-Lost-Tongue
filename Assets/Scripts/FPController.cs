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
    private float yRotation = 0f;

    // View mode and transition
    private bool isFirstPerson = true;
    private bool isTransitioning = false;
    private bool isStartingSkyView = true;
    private bool isPostTransitionFrame = false;
    private float transitionDuration = 2f;
    private float transitionTime = 0f;
    private Vector3 firstPersonPos = new Vector3(0f, 0.95f, 0f);
    private Vector3 thirdPersonPos = new Vector3(0f, 0.95f, -5f);
    private Quaternion skyRotation = Quaternion.Euler(-90f, 0f, 0f);
    private Quaternion thirdPersonRot = Quaternion.Euler(10f, 0f, 0f);

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform not assigned!");
        }
        Cursor.lockState = CursorLockMode.Locked;

        cameraTransform.localPosition = firstPersonPos;
        cameraTransform.localRotation = skyRotation;
        isFirstPerson = true;
        isStartingSkyView = true;

        xRotation = -90f;
        yRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveDirection = isFirstPerson ? transform.forward : cameraTransform.forward;
        moveDirection.y = 0f;
        moveDirection = moveDirection.normalized;
        Vector3 move = moveDirection * z + cameraTransform.right * x;
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
        if (!isStartingSkyView && cameraTransform != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            if (isFirstPerson && !isTransitioning)
            {
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                transform.Rotate(Vector3.up * mouseX);
            }
            else if (!isTransitioning && !isPostTransitionFrame)
            {
                yRotation += mouseX;
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 30f); // Stricter limit
                UpdateThirdPersonCamera();
            }
        }

        // Force sky view
        if (isStartingSkyView && cameraTransform != null)
        {
            cameraTransform.localRotation = skyRotation;
        }

        // Transition
        if (isTransitioning)
        {
            transitionTime += Time.deltaTime;
            float t = transitionTime / transitionDuration;
            if (t >= 1f)
            {
                t = 1f;
                isTransitioning = false;
                xRotation = thirdPersonRot.eulerAngles.x; // 10f
                yRotation = transform.eulerAngles.y;
                cameraTransform.position = transform.position + thirdPersonRot * thirdPersonPos;
                cameraTransform.localRotation = thirdPersonRot;
                isPostTransitionFrame = true;
            }
            else
            {
                cameraTransform.localPosition = Vector3.Lerp(firstPersonPos, thirdPersonPos, t);
                cameraTransform.localRotation = Quaternion.Slerp(skyRotation, thirdPersonRot, t);
            }
        }

        // Clear buffer frame
        if (isPostTransitionFrame)
        {
            isPostTransitionFrame = false;
        }

        // Test trigger - replace with New Button / Continue Button logic OnClick()
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartTransitionToThirdPerson();
        }
    }

    void UpdateThirdPersonCamera()
    {
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        Vector3 newPosition = transform.position + rotation * thirdPersonPos;

        // Prevent camera from going below character base (Y=0)
        if (newPosition.y < transform.position.y)
        {
            newPosition.y = transform.position.y; // Clamp to character base
        }

        cameraTransform.position = newPosition;
        cameraTransform.localRotation = rotation;
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
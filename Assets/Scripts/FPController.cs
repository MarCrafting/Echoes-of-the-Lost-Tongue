using UnityEngine;
using UnityEngine.UI;

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
    private float transitionDuration = 2f;
    private float transitionTime = 0f;
    private Vector3 firstPersonPos = new Vector3(0f, 0.95f, 0f);
    private Vector3 thirdPersonPos = new Vector3(0f, 0.95f, -5f);
    private Quaternion skyRotation = Quaternion.Euler(-90f, 0f, 0f);
    private Quaternion thirdPersonRot = Quaternion.Euler(10f, 0f, 0f);

    // UI
    public Button startGameButton;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform not assigned!");
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        cameraTransform.localPosition = firstPersonPos;
        cameraTransform.localRotation = skyRotation;
        isFirstPerson = true;
        isStartingSkyView = true;

        xRotation = -90f;
        yRotation = transform.eulerAngles.y;

        if (startGameButton != null)
        {
            startGameButton.gameObject.SetActive(true);
            startGameButton.onClick.AddListener(StartGame);
        }
        else
        {
            Debug.LogError("Start Game Button not assigned!");
        }
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
            else if (!isTransitioning)
            {
                yRotation += mouseX;
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 30f);
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
                yRotation = transform.eulerAngles.y;      // 0 if player Y=0

                Quaternion centeredRotation = Quaternion.Euler(xRotation, yRotation, 0f);
                Vector3 offset = centeredRotation * thirdPersonPos;
                cameraTransform.position = transform.position + offset;
                cameraTransform.localRotation = centeredRotation;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                // Interpolate world position relative to player
                Quaternion currentRotation = Quaternion.Slerp(skyRotation, thirdPersonRot, t);
                Vector3 currentOffset = Vector3.Lerp(firstPersonPos, thirdPersonPos, t);
                cameraTransform.position = transform.position + currentRotation * currentOffset;
                cameraTransform.localRotation = currentRotation;
            }
        }
    }

    void UpdateThirdPersonCamera()
    {
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        Vector3 newPosition = transform.position + rotation * thirdPersonPos;
        if (newPosition.y < transform.position.y)
        {
            newPosition.y = transform.position.y;
        }
        cameraTransform.position = newPosition;
        cameraTransform.localRotation = rotation;
    }

    public void StartGame()
    {
        if (!isTransitioning)
        {
            if (startGameButton != null)
            {
                startGameButton.gameObject.SetActive(false);
            }
            isStartingSkyView = false;
            isTransitioning = true;
            transitionTime = 0f;
            isFirstPerson = false;
        }
    }
}
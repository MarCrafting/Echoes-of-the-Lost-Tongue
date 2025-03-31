using UnityEngine;
using TMPro;

public class InteractableProp : MonoBehaviour
{
    public float interactionDistance = 2f;
    public string interactionText = "Press E to interact";
    public GameObject uiTextObject;
    public GameObject lessonContainer;
    public FPController fpsController; // Changed from FPSController to FPController

    private TextMeshProUGUI displayText;
    private Transform player;
    private bool isPlayerNear = false;

    void Start()
    {
        if (uiTextObject != null)
        {
            displayText = uiTextObject.GetComponent<TextMeshProUGUI>();
            if (displayText != null)
            {
                displayText.text = "";
                Debug.Log("Interaction prompt initialized, text cleared.");
            }
            else
            {
                Debug.LogError($"uiTextObject ({uiTextObject.name}) does not have a TextMeshProUGUI component!");
            }
        }
        else
        {
            Debug.LogError("uiTextObject is not assigned in the Inspector on " + gameObject.name);
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("No GameObject with tag 'Player' found in the scene!");
        }
        else
        {
            if (fpsController == null)
            {
                Debug.LogError("FPController not assigned in the Inspector on " + gameObject.name);
            }
        }

        if (lessonContainer != null)
        {
            lessonContainer.SetActive(false);
            Debug.Log("LessonContainer initialized: " + lessonContainer.name);
        }
        else
        {
            Debug.LogWarning("lessonContainer is not assigned in the Inspector on " + gameObject.name);
        }
    }

    void Update()
    {
        if (player == null || fpsController == null) return;

        // Suppress text during sky view or transition
        if (fpsController.isStartingSkyView || fpsController.isTransitioning)
        {
            if (displayText != null && displayText.text != "")
            {
                displayText.text = "";
                Debug.Log("Sky view or transition active, text cleared.");
            }
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        //Debug.Log($"Distance to player: {distance} (interactionDistance: {interactionDistance})");

        if (distance <= interactionDistance)
        {
            if (!isPlayerNear)
            {
                Debug.Log("Player entered range.");
            }
            isPlayerNear = true;

            // Show text only if lesson container is inactive
            if (displayText != null)
            {
                if (!lessonContainer.activeSelf)
                {
                    displayText.text = interactionText;
                    Debug.Log("Text set to: " + interactionText);
                }
                else
                {
                    displayText.text = "";
                    Debug.Log("Lesson container active, text cleared.");
                }
            }

            // Toggle lesson container with E
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!lessonContainer.activeSelf)
                {
                    Debug.Log("E pressed, activating lesson container...");
                    ToggleLessonContainer(true); // Activate
                }
                else
                {
                    Debug.Log("E pressed again, deactivating lesson container...");
                    ToggleLessonContainer(false); // Deactivate
                }
            }
        }
        else
        {
            if (isPlayerNear)
            {
                Debug.Log("Player left range.");
            }
            isPlayerNear = false;
            if (displayText != null)
            {
                displayText.text = "";
                Debug.Log("Text cleared (out of range).");
            }
        }
    }

    void ToggleLessonContainer(bool activate)
    {
        if (lessonContainer != null && fpsController != null)
        {
            lessonContainer.SetActive(activate);
            if (activate)
            {
                // Disable player input and show cursor
                fpsController.isInputEnabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                if (displayText != null)
                {
                    displayText.text = "";
                    Debug.Log("Text cleared on activation, input disabled.");
                }
            }
            else
            {
                // Re-enable player input and lock cursor
                fpsController.isInputEnabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (displayText != null && isPlayerNear)
                {
                    displayText.text = interactionText;
                    Debug.Log("Text restored on deactivation: " + interactionText);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
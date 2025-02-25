using UnityEngine;

public class ShortcutKeyHandler : MonoBehaviour
{
    public HiraganaDisplay spellingDisplay;

    private void Update()
    {   
        // Replay Audio
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Replay Audio Triggered");
            spellingDisplay.ReplayAudio();
        }

        // Translate Text to English
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Translate Toggle Triggered");
            spellingDisplay.ToggleTranslation();
        }
    }
}

using UnityEngine;

public class ShortcutKeyHandler : MonoBehaviour
{
    public HiraganaDisplay shortcutDisplay;

    private void Update()
    {   
        // Replay Audio
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Replay Audio Triggered");
            shortcutDisplay.ReplayAudio();
        }

        // Translate Text to English
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Translate Toggle Triggered");
            shortcutDisplay.ToggleTranslation();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Next Section Triggered");
            shortcutDisplay.NextSection();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("Previous Section Triggered");
            shortcutDisplay.PreviousSection();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ShortcutPopupManager : MonoBehaviour
{
    public GameObject shortcutPopup; // Reference to the shortcut popup panel

    private void Start()
    {
        shortcutPopup.SetActive(false); // Ensure popup is hidden at start
    }

    public void ToggleShortcutPopup()
    {
        shortcutPopup.SetActive(!shortcutPopup.activeSelf);
    }
}

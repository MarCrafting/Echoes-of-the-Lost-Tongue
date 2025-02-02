using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerClick;
        Debug.Log("Clicked Object: " + clickedObject.name);

        if (clickedObject.name == "New Btn")
        {
            Debug.Log("New Button Clicked! Loading Choose Language Scene...");
            SceneManager.LoadScene("Choose Language Scene"); //Case-sensative scene name
        }
        else if (clickedObject.name == "Quit Btn")
        {
            Debug.Log("Quit Button Clicked! Exiting Game...");
            Application.Quit();
        }
        else if (clickedObject.name == "Continue Btn")
        {
            Debug.Log("Continue Button Clicked! (Functionality to be implemented)");
            // Add continue game functionality here
        }
        else if (clickedObject.name == "Settings Btn")
        {
            Debug.Log("Settings Button Clicked! Loading Settings Scene...");
            SceneManager.LoadScene("Settings Screen Scene");
        }
        else if (clickedObject.name == "Japanese Select Btn")
        {
            Debug.Log("Language Select (Japanese) Button Clicked! Loading Tutorial Question Scene...");
            SceneManager.LoadScene("Tutorial Question Scene");
        }
    }
}

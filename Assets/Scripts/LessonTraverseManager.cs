using UnityEngine;

public class LessonTraverseManager : MonoBehaviour
{
    public GameObject lessonBackCanvas;  // reference to back side of cards (canvas)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Hide at start
        lessonBackCanvas.SetActive(false);

    }

    public void ShowBack()
    {
        lessonBackCanvas.SetActive(true); // show
    }

    public void HideBack()
    {
        lessonBackCanvas.SetActive(false); // hide back side
    }
}

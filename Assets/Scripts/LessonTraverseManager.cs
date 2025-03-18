using UnityEngine;

public class LessonTraverseManager : MonoBehaviour
{
    public GameObject lessonBackCanvas;  // reference to back side of cards (canvas)
    public GameObject lessonFrontPrevButton;    // reference to front side previous button
    private HiraganaDisplay display;    // reference to hiragana deck

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Hide at start
        lessonFrontPrevButton.SetActive(false);
        lessonBackCanvas.SetActive(false);

    }

    void Update()
    {
        // Hide prev button on front of first card
        if (display.currentCardIndex > 0)
            lessonFrontPrevButton.SetActive(true);
        else
            lessonFrontPrevButton.SetActive(false);   
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

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialQuestion : MonoBehaviour
{
    public TMP_Text questionText;
    public Button[] answerButtons;
    private string correctAnswer;
    private int tries;

    void Start()
    {
        // Load the question at the start
        LoadQuestion();
    }

    void LoadQuestion()
    {
        // Define the question, choices, and correct answer
        string question = "What is the translation for あ?";
        string[] choices = { "a", "i", "u", "e" };
        correctAnswer = "a";
        tries = 0;

        // Display the question text in the UI
        questionText.text = question;

        // Display the choices on each answer button
        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Get the TextMeshPro component from the button and set the text to the choice
            TMP_Text buttonText = answerButtons[i].GetComponentInChildren<TMP_Text>();
            buttonText.text = choices[i];
            int index = i; // Capture index for the lambda expression

            // Remove any previous click listeners to prevent multiple event stacking
            answerButtons[i].onClick.RemoveAllListeners();

            // Add a click listener that checks if the selected answer is correct
            answerButtons[i].onClick.AddListener(() => CheckAnswer(choices[index]));
        }
    }

    // Method to check if the selected answer is correct
    void CheckAnswer(string selectedAnswer)
    {
        // If the selected answer is correct
        if (selectedAnswer == correctAnswer)
        {
            Debug.Log("Correct!"); // Output to the console
            questionText.text = "Correct!"; // Display success message in the UI
        }
        else
        {
            // Increment the number of tries
            tries++;

            // If the user has more attempts left
            if (tries < 2)
            {
                Debug.Log("Wrong. Please try again."); // Output to the console
                questionText.text = "Wrong. Please try again."; // Prompt the user to try again
            }
            else
            {
                // If no attempts are left, display the correct answer
                Debug.Log($"Wrong. The correct answer is: {correctAnswer}"); // Output to the console
                questionText.text = $"Wrong. The correct answer is: {correctAnswer}"; // Display correct answer in UI
            }
        }
    }
}
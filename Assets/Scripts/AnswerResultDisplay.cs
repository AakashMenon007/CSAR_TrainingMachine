using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnswerResultDisplay : MonoBehaviour
{
    [Header("Canvas Elements")]
    public GameObject resultCanvas;                  // Canvas to show results
    public TMP_Text totalScoreText;                  
    public TMP_Text answerListText;                  

    [Header("References")]
    public AirtableManager airtableManager;          

    private readonly int[] correctAnswers = new int[8] { 2, 3, 3, 3, 2, 3, 3, 3 };

    public void ShowResults()
    {
        int totalCorrect = 0;
        string answerFeedback = "";

        string[] userAnswers = new string[8] {
            airtableManager.Question1,
            airtableManager.Question2,
            airtableManager.Question3,
            airtableManager.Question4,
            airtableManager.Question5,
            airtableManager.Question6,
            airtableManager.Question7,
            airtableManager.Question8
        };

        for (int i = 0; i < 8; i++)
        {
            int userAnswer;
            //Debug.Log($"User Answer for Q{i + 1}: {userAnswers[i]}");
            if (int.TryParse(userAnswers[i], out userAnswer))
            {
                bool isCorrect = userAnswer == correctAnswers[i];
                if (isCorrect) totalCorrect++;

                answerFeedback += $"Q{i + 1}: Your Answer = {userAnswer}, Correct = {correctAnswers[i]} - " +
                                  (isCorrect ? "<color=green>Correct</color>" : "<color=red>Wrong</color>") + "\n";
            }
            else
            {
                answerFeedback += $"Q{i + 1}: Invalid response\n";
            }
        }

        totalScoreText.text = $"Total Correct Answers: {totalCorrect}/8";
        answerListText.text = answerFeedback;

        resultCanvas.SetActive(true);
    }
}

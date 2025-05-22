using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostAssessmentController : MonoBehaviour
{
    [Header("Canvas Control")]
    public GameObject postAssessmentCanvas;

    private void Start()
    {
        postAssessmentCanvas.SetActive(false);
    }

    public void ShowCanvas()
    {
        postAssessmentCanvas.SetActive(true);
    }

    // Button: Close App
    public void OnExitApp()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Button: Restart Quiz Scene
    public void OnRestartAssessment()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Button: Go to Knowledge Base
    public void OnGoToKnowledgeBase()
    {
        SceneManager.LoadScene("2.KnowledgeBuilding"); // Replace with your actual scene name
    }
}

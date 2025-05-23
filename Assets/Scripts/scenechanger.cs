using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load. Leave empty to load the next scene in build settings.")]
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is tagged as "Player".
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                // Load the specified scene by name.
                if (Application.CanStreamedLevelBeLoaded(sceneName))
                {
                    SceneManager.LoadScene(sceneName);
                }
                else
                {
                    Debug.LogError($"Scene '{sceneName}' cannot be found. Please check the build settings.");
                }
            }
            else
            {
                // Load the next scene in the build settings.
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                int nextSceneIndex = currentSceneIndex + 1;

                if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(nextSceneIndex);
                }
                else
                {
                    Debug.LogWarning("No more scenes in the build settings to load.");
                }
            }
        }
    }
}

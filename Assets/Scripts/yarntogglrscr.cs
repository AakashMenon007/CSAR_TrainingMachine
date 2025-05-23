using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class ActivateDialogueAndObjectWithToggle : MonoBehaviour
{
    [Tooltip("The DialogueRunner used to run the Yarn dialogue.")]
    public DialogueRunner dialogueRunner;

    [Tooltip("The title of the Yarn dialogue node to activate.")]
    public string dialogueNodeTitle;

    [Tooltip("The GameObject to enable when the toggle is activated.")]
    public GameObject objectToEnable;

    [Tooltip("The Toggle UI component.")]
    public Toggle toggle;

    void Start()
    {
        if (toggle != null)
        {
            // Add a listener to the toggle to call the method when toggled on
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }
        else
        {
            Debug.LogWarning("Toggle is not assigned.");
        }
    }

    void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            // Start the Yarn dialogue
            if (dialogueRunner != null && !string.IsNullOrEmpty(dialogueNodeTitle))
            {
                dialogueRunner.StartDialogue(dialogueNodeTitle);
                Debug.Log($"Starting dialogue: {dialogueNodeTitle}");
            }
            else
            {
                Debug.LogWarning("DialogueRunner or dialogueNodeTitle is not set.");
            }

            // Enable the GameObject
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true);
                Debug.Log($"GameObject '{objectToEnable.name}' is now active.");
            }
            else
            {
                Debug.LogWarning("Object to enable is not set.");
            }

            // Optionally reset the toggle
            toggle.isOn = false;
        }
    }
}

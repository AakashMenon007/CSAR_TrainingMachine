using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using System.Collections.Generic;

public class YarnToggleTrigger : MonoBehaviour
{
    [Header("UI and Yarn Settings")]
    public Toggle myToggle;
    public DialogueRunner dialogueRunner;
    public string nodeTitle = "YourNodeTitle"; // Replace with your actual node title

    [Header("GameObjects to Activate")]
    public List<GameObject> objectsToActivate;

    // Static reference to track previously activated objects
    private static List<GameObject> previouslyActivatedObjects = new List<GameObject>();

    void Start()
    {
        // Add a listener to the toggle
        myToggle.onValueChanged.AddListener(OnToggleChanged);

        // Ensure all specified GameObjects are initially inactive
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            // Deactivate previously activated objects
            foreach (GameObject obj in previouslyActivatedObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }

            // Trigger the Yarn node
            dialogueRunner.StartDialogue(nodeTitle);

            // Activate new objects and update the reference
            previouslyActivatedObjects = new List<GameObject>();
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                    previouslyActivatedObjects.Add(obj);
                }
            }

            // Optionally, reset the toggle to off
            myToggle.isOn = false;
        }
    }
}

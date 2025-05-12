using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class YarnToggleTrigger : MonoBehaviour
{
    public Toggle myToggle;
    public DialogueRunner dialogueRunner;
    public string nodeTitle = "YourNodeTitle"; // Replace with your actual node title

    void Start()
    {
        myToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            dialogueRunner.StartDialogue(nodeTitle); // Triggers the Yarn node
            // Optionally, reset the toggle if you want
            myToggle.isOn = false;
        }
    }
}

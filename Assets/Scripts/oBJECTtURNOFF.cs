using UnityEngine;

public class DelayedDeactivate : MonoBehaviour
{
    [Tooltip("GameObject to be turned off after delay.")]
    public GameObject objectToTurnOff;

    [Tooltip("GameObject to be turned on after delay.")]
    public GameObject objectToTurnOn;

    [Tooltip("Time in seconds before the GameObject is turned off.")]
    public float delayTime = 2.0f;

    void Start()
    {
        // Start the delayed deactivation process
        Invoke("TurnOffAndTurnOn", delayTime);
    }

    void TurnOffAndTurnOn()
    {
        // Deactivate the object to turn off
        if (objectToTurnOff != null)
        {
            objectToTurnOff.SetActive(false);
        }

        // Activate the object to turn on
        if (objectToTurnOn != null)
        {
            objectToTurnOn.SetActive(true);
        }
    }
}


using UnityEngine;
using System.Collections.Generic;

public class DelayedDeactivate : MonoBehaviour
{
    [Tooltip("List of GameObjects to be turned off after delay.")]
    public List<GameObject> objectsToTurnOff = new List<GameObject>();

    [Tooltip("GameObject to be turned on after delay.")]
    public GameObject objectToTurnOn;

    [Tooltip("Time in seconds before the GameObjects are turned off.")]
    public float delayTime = 2.0f;

    void Start()
    {
        // Start the delayed deactivation process
        Invoke("TurnOffAndTurnOn", delayTime);
    }

    void TurnOffAndTurnOn()
    {
        // Deactivate all objects in the list
        foreach (GameObject obj in objectsToTurnOff)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        // Activate the object to turn on
        if (objectToTurnOn != null)
        {
            objectToTurnOn.SetActive(true);
        }
    }
}

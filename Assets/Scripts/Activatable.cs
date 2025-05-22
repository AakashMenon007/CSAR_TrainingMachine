using UnityEngine;

public class Activatable : MonoBehaviour
{
    /// <summary>
    /// Activates the GameObject.
    /// </summary>
    public void Activate()
    {
        gameObject.SetActive(true);
        Debug.Log($"{gameObject.name} has been activated!");
    }

    /// <summary>
    /// Deactivates the GameObject.
    /// </summary>
    public void Deactivate()
    {
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} has been deactivated!");
    }

    /// <summary>
    /// Toggles the active state of the GameObject.
    /// </summary>
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        string state = gameObject.activeSelf ? "activated" : "deactivated";
        Debug.Log($"{gameObject.name} has been {state}!");
    }
}

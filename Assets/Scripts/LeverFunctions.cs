using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class XRLeverIndicator : MonoBehaviour
{
    [Tooltip("GameObject representing the red light indicator.")]
    [SerializeField] private GameObject redIndicator; // Red light object
    [Tooltip("GameObject representing the green light indicator.")]
    [SerializeField] private GameObject greenIndicator; // Green light object
    [Tooltip("Material with emission for the active red light.")]
    [SerializeField] private Material activeRedMaterial;
    [Tooltip("Material with emission for the active green light.")]
    [SerializeField] private Material activeGreenMaterial;
    [Tooltip("Default material for the red light.")]
    [SerializeField] private Material defaultRedMaterial;
    [Tooltip("Default material for the green light.")]
    [SerializeField] private Material defaultGreenMaterial;

    // Function called when the lever is activated
    public void OnLeverActivated()
    {
        if (greenIndicator != null && redIndicator != null)
        {
            // Activate green light and set red light to default
            greenIndicator.GetComponent<Renderer>().material = activeGreenMaterial;
            redIndicator.GetComponent<Renderer>().material = defaultRedMaterial;
        }
        else
        {
            Debug.LogError("Green or Red indicator GameObjects are not assigned.");
        }
    }

    // Function called when the lever is deactivated
    public void OnLeverDeactivated()
    {
        if (greenIndicator != null && redIndicator != null)
        {
            // Activate red light and set green light to default
            redIndicator.GetComponent<Renderer>().material = activeRedMaterial;
            greenIndicator.GetComponent<Renderer>().material = defaultGreenMaterial;
        }
        else
        {
            Debug.LogError("Green or Red indicator GameObjects are not assigned.");
        }
    }
}

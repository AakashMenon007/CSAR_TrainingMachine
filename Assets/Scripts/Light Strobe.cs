using UnityEngine;

public class LightStrobe : MonoBehaviour
{
    [Tooltip("Time in seconds for the light to toggle intensity.")]
    public float strobeInterval = 0.5f;  // How frequently the light toggles
    public float maxIntensity = 5f;       // Maximum light intensity
    public float minIntensity = 0f;       // Minimum light intensity (off)

    private Light pointLight;

    void Start()
    {
        // Get the Light component attached to the same GameObject
        pointLight = GetComponent<Light>();

        if (pointLight == null)
        {
            Debug.LogError("No Light component found on this GameObject.");
        }
        else
        {
            // Start the strobe effect
            InvokeRepeating("ToggleLight", 0f, strobeInterval);
        }
    }

    void ToggleLight()
    {
        // Toggle between maximum and minimum light intensity
        pointLight.intensity = pointLight.intensity == maxIntensity ? minIntensity : maxIntensity;
    }
}

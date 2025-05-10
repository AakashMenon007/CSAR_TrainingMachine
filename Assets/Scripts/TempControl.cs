using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSliderTemperature : MonoBehaviour
{
    [SerializeField] private XRSlider slider; // Reference to the XR slider
    [Tooltip("Drag and drop the TextMeshPro 3D object here to display the temperature")] // Tooltip for clarity
    [SerializeField] private TextMeshPro temperatureDisplay; // Text to update (3D TextMeshPro)

    private float previousValue = -1f; // To track the last value and avoid unnecessary updates

    private void Start()
    {
        if (slider == null)
        {
            Debug.LogError("XR Slider is not assigned.");
        }

        if (temperatureDisplay == null)
        {
            Debug.LogError("Temperature Display Text is not assigned.");
        }

        UpdateTemperature(slider != null ? slider.value : 0f); // Initialize the temperature display
    }

    private void Update()
    {
        if (slider == null || temperatureDisplay == null) return;

        float currentValue = slider.value;

        // Update only if the slider value has changed
        if (Mathf.Abs(currentValue - previousValue) > Mathf.Epsilon)
        {
            UpdateTemperature(currentValue);
            previousValue = currentValue;
        }
    }

    private void UpdateTemperature(float value)
    {
        int temperature = Mathf.RoundToInt(value * 60); // Scale value to 0-60 degrees
        temperatureDisplay.text = temperature + "°C"; // Update text
    }
}

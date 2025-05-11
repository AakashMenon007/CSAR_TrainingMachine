using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSliderTemperature : MonoBehaviour
{
    [SerializeField] private XRSlider slider; // Reference to the XR slider
    [Tooltip("Drag and drop the TextMeshPro 3D object here to display the temperature")]
    [SerializeField] private TextMeshPro temperatureDisplay; // Text to update (3D TextMeshPro)

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

        if (redIndicator == null || greenIndicator == null)
        {
            Debug.LogError("Indicator GameObjects are not assigned.");
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

        UpdateIndicators(temperature); // Update light indicators based on the temperature
    }

    private void UpdateIndicators(int temperature)
    {
        if (redIndicator == null || greenIndicator == null) return;

        // Check if the temperature is within the range 25-28
        if (temperature >= 25 && temperature <= 28)
        {
            // Activate green light and set red light to default
            greenIndicator.GetComponent<Renderer>().material = activeGreenMaterial;
            redIndicator.GetComponent<Renderer>().material = defaultRedMaterial;
        }
        else
        {
            // Activate red light and set green light to default
            redIndicator.GetComponent<Renderer>().material = activeRedMaterial;
            greenIndicator.GetComponent<Renderer>().material = defaultGreenMaterial;
        }
    }
}

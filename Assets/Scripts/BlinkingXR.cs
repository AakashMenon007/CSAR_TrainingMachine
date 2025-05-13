using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using System.Collections;

public class BlinkingXR : MonoBehaviour
{
    [SerializeField] private XRSlider slider; // Reference to the XR slider
    [Tooltip("Drag and drop the TextMeshPro 3D object here to display the temperature")]
    [SerializeField] private TextMeshPro temperatureDisplay; // Text to update (3D TextMeshPro)

    [Tooltip("GameObject representing the red light indicator.")]
    [SerializeField] private GameObject redIndicator; // Red light object
    [Tooltip("GameObject representing the green light indicator.")]
    [SerializeField] private GameObject greenIndicator; // Green light object
    [Tooltip("GameObject representing the blinking light indicator.")]
    [SerializeField] private GameObject blinkingIndicator; // Blinking light object

    [Tooltip("Material with emission for the active red light.")]
    [SerializeField] private Material activeRedMaterial;
    [Tooltip("Material with emission for the active green light.")]
    [SerializeField] private Material activeGreenMaterial;
    [Tooltip("Material for the blinking light.")]
    [SerializeField] private Material blinkingMaterial;
    [Tooltip("Default material for the blinking light.")]
    [SerializeField] private Material defaultBlinkingMaterial;
    [Tooltip("Default material for the red light.")]
    [SerializeField] private Material defaultRedMaterial;
    [Tooltip("Default material for the green light.")]
    [SerializeField] private Material defaultGreenMaterial;

    private float previousValue = -1f; // To track the last value and avoid unnecessary updates
    private bool isBlinking = false; // To track the blinking state
    private Coroutine blinkingCoroutine; // To control the blinking

    private void Start()
    {
        if (slider == null) Debug.LogError("XR Slider is not assigned.");
        if (temperatureDisplay == null) Debug.LogError("Temperature Display Text is not assigned.");
        if (redIndicator == null || greenIndicator == null || blinkingIndicator == null)
            Debug.LogError("Indicator GameObjects are not assigned.");

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
        if (redIndicator == null || greenIndicator == null || blinkingIndicator == null) return;

        // Check if the temperature is within the range 25-28
        if (temperature >= 25 && temperature <= 28)
        {
            // Activate green light and set red light and blinking light to default
            greenIndicator.GetComponent<Renderer>().material = activeGreenMaterial;
            redIndicator.GetComponent<Renderer>().material = defaultRedMaterial;

            if (isBlinking)
            {
                StopBlinking();
            }
        }
        else
        {
            // Activate red light and start blinking
            redIndicator.GetComponent<Renderer>().material = activeRedMaterial;
            greenIndicator.GetComponent<Renderer>().material = defaultGreenMaterial;

            if (!isBlinking)
            {
                StartBlinking();
            }
        }
    }

    private void StartBlinking()
    {
        isBlinking = true;
        blinkingCoroutine = StartCoroutine(BlinkingRoutine());
    }

    private void StopBlinking()
    {
        isBlinking = false;
        if (blinkingCoroutine != null)
        {
            StopCoroutine(blinkingCoroutine);
        }

        // Ensure the blinking indicator is reset to the default material
        blinkingIndicator.GetComponent<Renderer>().material = defaultBlinkingMaterial;
    }

    private IEnumerator BlinkingRoutine()
    {
        Renderer blinkingRenderer = blinkingIndicator.GetComponent<Renderer>();

        while (isBlinking)
        {
            // Turn on blinking material
            blinkingRenderer.material = blinkingMaterial;

            // Wait for 0.3 seconds
            yield return new WaitForSeconds(0.3f);

            // Turn off blinking material
            blinkingRenderer.material = defaultBlinkingMaterial;

            // Wait for 0.3 seconds
            yield return new WaitForSeconds(0.3f);
        }

        // Ensure the blinking indicator is reset to the default material when stopped
        blinkingRenderer.material = defaultBlinkingMaterial;
    }
}

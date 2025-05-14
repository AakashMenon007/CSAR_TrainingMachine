using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using System.Collections;

public class BlinkingPh : MonoBehaviour
{
    [SerializeField] private XRKnob knob; // Reference to the XRKnob
    [Tooltip("Drag and drop the TextMeshPro 3D object here to display the pH value")]
    [SerializeField] private TextMeshPro pHDisplay; // Text to update (3D TextMeshPro)

    [Tooltip("GameObject representing the red light indicator.")]
    [SerializeField] private GameObject redIndicator; // Red light object
    [Tooltip("GameObject representing the green light indicator.")]
    [SerializeField] private GameObject greenIndicator; // Green light object
    [Tooltip("GameObject representing the blinking light.")]
    [SerializeField] private GameObject blinkingIndicator; // Blinking light object

    [Tooltip("Material with emission for the active red light.")]
    [SerializeField] private Material activeRedMaterial;
    [Tooltip("Material with emission for the active green light.")]
    [SerializeField] private Material activeGreenMaterial;
    [Tooltip("Material for the blinking light.")]
    [SerializeField] private Material blinkingMaterial;

    [Tooltip("Default material for the red light.")]
    [SerializeField] private Material defaultRedMaterial;
    [Tooltip("Default material for the green light.")]
    [SerializeField] private Material defaultGreenMaterial;
    [Tooltip("Default material for the blinking light.")]
    [SerializeField] private Material defaultBlinkingMaterial;

    private float previousValue = -999f; // To track the last value and avoid unnecessary updates
    private bool isBlinking = false; // To track the blinking state
    private Coroutine blinkingCoroutine; // To control the blinking

    private void Start()
    {
        if (knob == null) Debug.LogError("XR Knob is not assigned.");
        if (pHDisplay == null) Debug.LogError("pH Display Text is not assigned.");
        if (redIndicator == null || greenIndicator == null || blinkingIndicator == null)
            Debug.LogError("Indicator GameObjects are not assigned.");

        // Initialize the pH display
        UpdatePH(knob != null ? knob.value : 0f);
    }

    private void Update()
    {
        if (knob == null || pHDisplay == null) return;

        float currentValue = knob.value;

        // Update only if the knob value has changed
        if (Mathf.Abs(currentValue - previousValue) > Mathf.Epsilon)
        {
            UpdatePH(currentValue);
            previousValue = currentValue;
        }
    }

    private void UpdatePH(float value)
    {
        // Map knob value (0..1) to -15..15
        int ph = Mathf.RoundToInt(Mathf.Lerp(-15f, 15f, value));
        pHDisplay.text = ph + " pH";

        // Update the material indicators
        UpdateIndicators(ph);
    }

    private void UpdateIndicators(int ph)
    {
        if (redIndicator == null || greenIndicator == null || blinkingIndicator == null) return;

        // Check if the pH value is within the range 6.5 to 8.5
        if (ph >= 6.5f && ph <= 8.5f)
        {
            // Activate green light and turn off red and blinking lights
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
            // Apply the blinking material
            blinkingRenderer.material = blinkingMaterial;

            // Wait for 0.3 seconds (faster blinking pace)
            yield return new WaitForSeconds(0.3f);

            // Remove the blinking material (reset to default)
            blinkingRenderer.material = defaultBlinkingMaterial;

            // Wait for 0.3 seconds before toggling again
            yield return new WaitForSeconds(0.3f);
        }

        // Ensure the blinking indicator is reset to the default material when stopping
        blinkingRenderer.material = defaultBlinkingMaterial;
    }

    public void OnKnobValueChanged(float value)
    {
        UpdatePH(value);
        previousValue = value;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class XRKnobPH : MonoBehaviour
{
    [SerializeField] private XRKnob knob; // Reference to the XRKnob
    [Tooltip("Drag and drop the TextMeshPro 3D object here to display the pH value")]
    [SerializeField] private TextMeshPro pHDisplay; // Text to update (3D TextMeshPro)

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

    private float previousValue = -999f; // To track the last value and avoid unnecessary updates

    private void Start()
    {
        if (knob == null)
        {
            Debug.LogError("XR Knob is not assigned.");
        }

        if (pHDisplay == null)
        {
            Debug.LogError("pH Display Text is not assigned.");
        }

        if (redIndicator == null || greenIndicator == null)
        {
            Debug.LogError("Indicator GameObjects are not assigned.");
        }

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
        if (redIndicator == null || greenIndicator == null) return;

        // Check if the pH value is within the range 6.5 to 8.5
        if (ph >= 6.5f && ph <= 8.5f)
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

    public void OnKnobValueChanged(float value)
    {
        UpdatePH(value);
        previousValue = value;
    }
}

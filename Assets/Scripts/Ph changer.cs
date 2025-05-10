using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class XRKnobPH : MonoBehaviour
{
    [SerializeField] private XRKnob knob; // Reference to the XRKnob
    [Tooltip("Drag and drop the TextMeshPro 3D object here to display the pH value")]
    [SerializeField] private TextMeshPro pHDisplay; // Text to update (3D TextMeshPro)

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
    }
    public void OnKnobValueChanged(float value)
    {
        UpdatePH(value);
        previousValue = value;
    }

}

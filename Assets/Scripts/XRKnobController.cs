using UnityEngine;
using NovaSamples.UIControls;


public class XRKnobController : MonoBehaviour
{
    [Tooltip("The ProgressBar to control.")]
    public ProgressBar progressBar;

    private float previousValue = 0.5f; // Default knob value at the center (0.5).

    /// <summary>
    /// Called when the knob value changes.
    /// </summary>
    /// <param name="sliderValue">The new value of the knob (0 to 1).</param>
    public void OnKnobValueChanged(float sliderValue)
    {
        if (progressBar != null)
        {
            // Call the OnWheelValueChange method in ProgressBar
            progressBar.OnWheelValueChange(sliderValue, previousValue);
        }

        // Update the previous value for the next calculation
        previousValue = sliderValue;
    }
}

using Nova;
using System.Text;
using UnityEngine;

namespace NovaSamples.UIControls
{
    public enum ProgressBarStyle
    {
        Clockwise,
        CounterClockwise,
        Vertical,
        Horizontal,
    }

    public class ProgressBar : MonoBehaviour
    {
        const int MinDecimals = 0;
        const int MaxDecimals = 4;

        [SerializeField, Range(0, 400), Tooltip("The current PSI level.")]
        private float psiLevel = 50f; // Default to 50

        [Header("Progress Indicator")]
        [Tooltip("The UI Block 2D whose properties will be modified to indicate the progress state.")]
        public UIBlock2D Fill = null;

        [Tooltip("How the PSI level will be visualized.")]
        public ProgressBarStyle Style = ProgressBarStyle.Horizontal;

        [Header("Text Format")]
        [Tooltip("The Text Block used to display the PSI level.")]
        public TextBlock Text = null;

        [SerializeField, Range(MinDecimals, MaxDecimals), Tooltip("The decimal precision of the text string.")]
        private int decimalCount = 0;

        private StringBuilder textFormatBuilder = new StringBuilder();
        private string textFormat = string.Empty;

        /// <summary>
        /// Gets or sets the PSI level and updates visuals accordingly.
        /// </summary>
        public float PsiLevel
        {
            get => psiLevel;
            set
            {
                psiLevel = Mathf.Clamp(value, 0, 400);
                UpdateProgressVisuals();
            }
        }

        private void Awake()
        {
            UpdateTextFormat();
            UpdateProgressVisuals();
        }

        /// <summary>
        /// Updates the fill and text visuals.
        /// </summary>
        private void UpdateProgressVisuals()
        {
            UpdateFillVisual();
            UpdateTextVisual();
        }

        /// <summary>
        /// Updates the <see cref="Fill"/> visual based on <see cref="PsiLevel"/>.
        /// </summary>
        private void UpdateFillVisual()
        {
            if (Fill == null)
            {
                return;
            }

            float normalizedValue = PsiLevel / 400f; // Normalize PSI level to a range of 0 to 1.

            switch (Style)
            {
                case ProgressBarStyle.Clockwise:
                    Fill.RadialFill.Enabled = true;
                    Fill.RadialFill.FillAngle = normalizedValue * -360f;
                    break;
                case ProgressBarStyle.CounterClockwise:
                    Fill.RadialFill.Enabled = true;
                    Fill.RadialFill.FillAngle = normalizedValue * 360f;
                    break;
                case ProgressBarStyle.Vertical:
                    Fill.AutoSize.Y = AutoSize.None;
                    Fill.Size.Y = Length.Percentage(normalizedValue * (1f - Fill.CalculatedMargin.Y.Sum().Percent));
                    break;
                case ProgressBarStyle.Horizontal:
                    Fill.AutoSize.X = AutoSize.None;
                    Fill.Size.X = Length.Percentage(normalizedValue * (1f - Fill.CalculatedMargin.X.Sum().Percent));
                    break;
            }
        }

        /// <summary>
        /// Updates the <see cref="Text"/> visual based on <see cref="PsiLevel"/>.
        /// </summary>
        private void UpdateTextVisual()
        {
            if (Text == null)
            {
                return;
            }

            float displayedValue = PsiLevel;
            string format = displayedValue == 0 ? "0" : textFormat;
            string text = string.Format(format, displayedValue) + " PSI";

            if (Text.Text == text)
            {
                return;
            }

            Text.Text = text;
        }

        /// <summary>
        /// Updates the desired text format based on the <see cref="decimalCount"/>.
        /// </summary>
        private void UpdateTextFormat()
        {
            textFormatBuilder.Clear();

            textFormatBuilder.Append("{0:0");
            if (decimalCount > 0)
            {
                textFormatBuilder.Append(".");
                for (int i = 0; i < decimalCount; ++i)
                {
                    textFormatBuilder.Append("0");
                }
            }
            textFormatBuilder.Append("}");
            textFormat = textFormatBuilder.ToString();
        }

        /// <summary>
        /// Public method to adjust the PSI level by a delta.
        /// </summary>
        public void ChangePsi(float delta)
        {
            PsiLevel += delta;
        }

        private void OnValidate()
        {
            UpdateTextFormat();
            PsiLevel = psiLevel;
        }

        /// <summary>
        /// Called when the wheel slider value changes.
        /// </summary>
        public void OnWheelValueChange(float sliderValue, float previousValue)
        {
            float delta = sliderValue - previousValue;
            ChangePsi(delta);
        }

        /// <summary>
        /// Call this method with the amount of PSI to increase or decrease.
        /// For example, OnKnobValueChanged(10) increases PSI by 10, OnKnobValueChanged(-5) decreases by 5.
        /// Turning right (positive delta) increases PSI, left (negative) decreases PSI.
        /// </summary>
        /// <param name="deltaPsi">The amount of PSI to add (positive or negative).</param>
        public void OnKnobValueChanged(float deltaPsi)
        {
            Debug.Log("Delta PSI received: " + deltaPsi);

            if (Mathf.Abs(deltaPsi) > 0.01f)
            {
                ChangePsi(deltaPsi); // Use as-is: right increases, left decreases
            }
        }
    }
}

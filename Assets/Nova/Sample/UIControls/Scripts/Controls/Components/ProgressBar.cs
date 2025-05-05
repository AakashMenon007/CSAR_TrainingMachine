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
        private float psiLevel = 50f;

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

        [Header("Knob Settings")]
        [SerializeField, Tooltip("PSI change per knob step")]
        private float stepSize = 5f;

        private StringBuilder textFormatBuilder = new StringBuilder();
        private string textFormat = string.Empty;

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

        private void UpdateProgressVisuals()
        {
            UpdateFillVisual();
            UpdateTextVisual();
        }

        private void UpdateFillVisual()
        {
            if (Fill == null) return;

            float normalizedValue = PsiLevel / 400f;

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

        private void UpdateTextVisual()
        {
            if (Text == null) return;

            string format = (PsiLevel == 0) ? "0" : textFormat;
            string newText = $"{string.Format(format, PsiLevel)} PSI";

            if (Text.Text != newText)
            {
                Text.Text = newText;
            }
        }

        private void UpdateTextFormat()
        {
            textFormatBuilder.Clear();
            textFormatBuilder.Append("{0:0");

            if (decimalCount > 0)
            {
                textFormatBuilder.Append(".");
                textFormatBuilder.Append('0', decimalCount);
            }

            textFormatBuilder.Append('}');
            textFormat = textFormatBuilder.ToString();
        }

        public void ChangePsi(float delta)
        {
            PsiLevel += delta;
        }

        private void OnValidate()
        {
            UpdateTextFormat();
            PsiLevel = psiLevel;
        }

        public void OnKnobIncrement(float _)
        {
            ChangePsi(stepSize);
        }

        public void OnKnobDecrement(float _)
        {
            ChangePsi(-stepSize);
        }
    }
}
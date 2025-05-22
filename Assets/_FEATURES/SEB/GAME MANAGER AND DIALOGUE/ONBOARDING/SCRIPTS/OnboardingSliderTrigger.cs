using UnityEngine;

namespace Amused.XR
{
    public class OnboardingSliderTrigger : MonoBehaviour
    {
        [Tooltip("The onboarding step at which the slider becomes active.")]
        public int validStep = 10;
        [Tooltip("Only allow progression after this much movement (0..1), or set to 0.01 for any movement.")]
        public float requiredValue = 0.01f;

        public OnboardingController onboardingController;

        private NPCInstructorController npc;
        private bool hasProceeded = false;
        private float initialValue = -1f; // to detect change

        private void Start()
        {
            npc = FindObjectOfType<NPCInstructorController>();
        }

        // Call this from your slider's value change event
        public void OnSliderChanged(float value)
        {
            if (hasProceeded)
                return;
            if (onboardingController.GetCurrentStep() != validStep)
                return;
            if (npc != null && npc.DialogueIsActive)
                return;

            // If this is the first time, set initial value and require more movement
            if (initialValue < 0f)
            {
                initialValue = value;
                return;
            }

            // Require enough movement from initialValue
            if (Mathf.Abs(value - initialValue) < requiredValue)
                return;

            hasProceeded = true;
            Debug.Log("[OnboardingSliderTrigger] Slider moved enough at correct step — proceeding.");
            onboardingController.ProceedToNextStep();
        }

        public void ResetTrigger()
        {
            hasProceeded = false;
            initialValue = -1f;
        }
    }
}

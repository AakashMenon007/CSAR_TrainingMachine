using UnityEngine;

namespace Amused.XR
{
    public class OnboardingRotatableTrigger : MonoBehaviour
    {
        [Tooltip("Step where this trigger is needed (e.g., 8 for valve, 9 for dial)")]
        public int validStep = 8;
        [Tooltip("Type for logs: 'valve', 'dial', etc.")]
        public string triggerType = "valve";
        [Tooltip("How far to turn (0–1). If you want to ignore, set to 0 or 0.01 for dial.")]
        public float requiredValue = 0.9f;

        public OnboardingController onboardingController;

        private NPCInstructorController npc;
        private bool hasProceeded = false;

        private void Start()
        {
            npc = FindObjectOfType<NPCInstructorController>();
        }

        // Call this from your rotatable's value change event (float between 0 and 1)
        public void OnRotated(float value)
        {
            if (hasProceeded)
                return;
            if (onboardingController.GetCurrentStep() != validStep)
                return;
            if (npc != null && npc.DialogueIsActive)
                return;

            // Only require reaching requiredValue if it's more than a minimal value (e.g. for dial, just use 0.01 or any interaction)
            if (requiredValue > 0.05f)
            {
                if (value < requiredValue)
                    return;
            }
            else if (value <= 0.01f) // if requiredValue not used, require any non-zero movement
            {
                return;
            }

            hasProceeded = true;
            Debug.Log($"[OnboardingRotatableTrigger] {triggerType} used (value: {value}) at correct step — proceeding.");
            onboardingController.ProceedToNextStep();
        }

        public void ResetTrigger()
        {
            hasProceeded = false;
        }
    }
}

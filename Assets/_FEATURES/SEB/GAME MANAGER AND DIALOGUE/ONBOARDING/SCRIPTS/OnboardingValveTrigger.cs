using UnityEngine;

namespace Amused.XR
{
    public class OnboardingValveTrigger : MonoBehaviour
    {
        [Tooltip("Step where valve is needed (e.g., 8)")]
        public int validStep = 8;
        [Tooltip("How far to turn valve (0–1)")]
        public float requiredValue = 0.9f;

        public OnboardingController onboardingController;

        private NPCInstructorController npc;
        private bool hasProceeded = false;

        private void Start()
        {
            npc = FindObjectOfType<NPCInstructorController>();
        }

        public void OnValveChanged(float value)
        {
            // Only work if not already used, and at correct step, and dialogue is done
            if (hasProceeded)
                return;
            if (onboardingController.GetCurrentStep() != validStep)
                return;
            if (npc != null && npc.DialogueIsActive)
                return;

            if (value >= requiredValue)
            {
                hasProceeded = true;
                Debug.Log("[OnboardingValveTrigger] Valve turned enough at right step — proceeding.");
                onboardingController.ProceedToNextStep();
            }
        }

        public void ResetTrigger()
        {
            hasProceeded = false;
        }
    }
}

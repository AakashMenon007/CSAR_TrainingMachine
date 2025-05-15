using UnityEngine;

namespace Amused.XR
{
    public class ValveTrigger : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float requiredValue = 0.9f;
        public int validStep = 9; // This is the step for onboarding_3d
        public OnboardingController onboardingController;

        private NPCInstructorController npc;
        private bool hasProceeded = false;

        private void Start()
        {
            npc = FindObjectOfType<NPCInstructorController>();
        }

        public void OnValveChanged(float value)
        {
            if (hasProceeded)
                return;

            if (onboardingController.GetCurrentStep() != validStep)
            {
                Debug.Log("[ValveTrigger] Ignored — wrong step.");
                return;
            }

            if (npc != null && npc.DialogueIsActive)
            {
                Debug.Log("[ValveTrigger] Ignored — dialogue still playing.");
                return;
            }

            if (value >= requiredValue)
            {
                hasProceeded = true;
                Debug.Log("[ValveTrigger] Correct step and value — proceeding.");
                onboardingController.ProceedToNextStep();
            }
        }
    }
}

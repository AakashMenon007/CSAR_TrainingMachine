using UnityEngine;

namespace Amused.XR
{
    public class LeverTrigger : MonoBehaviour
    {
        public OnboardingController onboardingController;
        public int validStep = 8;

        private NPCInstructorController npc;

        private void Start()
        {
            npc = FindObjectOfType<NPCInstructorController>();
        }

        public void OnLeverActivated()
        {


            if (onboardingController.GetCurrentStep() != validStep)
            {
                Debug.Log("[LeverTrigger] Ignored — wrong step.");
                return;
            }

            if (npc != null && npc.DialogueIsActive)
            {
                Debug.Log("[LeverTrigger] Ignored — dialogue not finished.");
                return;
            }

            Debug.Log("[LeverTrigger] Lever activated — proceeding to next step.");
            onboardingController.ProceedToNextStep();
        }
    }
}

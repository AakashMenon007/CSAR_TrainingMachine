using UnityEngine;

namespace Amused.XR
{
    public class OnboardingPhysicalButtonTrigger : MonoBehaviour
    {
        public OnboardingController onboardingController;

        public enum ButtonType { Yes, No }
        public ButtonType buttonType;

        [Tooltip("All steps where this button is allowed to work")]
        public int[] validSteps;

        public void OnPressed()
        {
            if (onboardingController == null)
            {
                Debug.LogWarning("[Button] OnboardingController not assigned.");
                return;
            }

            int currentStep = onboardingController.GetCurrentStep();

            if (!IsStepValid(currentStep))
            {
                Debug.Log($"[Button] Ignored press — current step is {currentStep}, not valid for this button.");
                return;
            }

            if (FindObjectOfType<NPCInstructorController>().DialogueIsActive)
            {
                Debug.Log("[Button] Ignored — NPC still speaking.");
                return;
            }

            switch (buttonType)
            {
                case ButtonType.Yes:
                    if (currentStep == 12)
                    {
                        Debug.Log("[Button] YES.");
                        FindObjectOfType<OnboardingStepsHandler>().ExecuteStep(14);
                    }
                    else
                    {
                        Debug.Log($"[Button] YES at step {currentStep} — proceeding normally.");
                        onboardingController.ProceedToNextStep();
                    }
                    break;

                case ButtonType.No:
                    Debug.Log("[Button] NO — proceeding to step 12 (restart line).");
                    onboardingController.ProceedToNextStep();
                    break;
            }

            //gameObject.SetActive(false);
        }

        private bool IsStepValid(int step)
        {
            foreach (int valid in validSteps)
                if (step == valid)
                    return true;
            return false;
        }
    }
}

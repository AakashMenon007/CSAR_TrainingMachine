using UnityEngine;

namespace Amused.XR
{
    public class OnboardingPhysicalButtonTrigger : MonoBehaviour
    {
        public OnboardingController onboardingController;

        public void OnPressed()
        {
            if (onboardingController == null)
            {
                Debug.LogWarning("[PhysicalButton] OnboardingController not assigned.");
                return;
            }

            int currentStep = onboardingController.GetCurrentStep();

            if (currentStep == 5 || currentStep == 12)
            {
                Debug.Log($"[PhysicalButton] Step {currentStep} valid — proceeding.");
                onboardingController.ProceedToNextStep();
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log($"[PhysicalButton] Ignored press at step {currentStep}.");
            }
        }
    }
}

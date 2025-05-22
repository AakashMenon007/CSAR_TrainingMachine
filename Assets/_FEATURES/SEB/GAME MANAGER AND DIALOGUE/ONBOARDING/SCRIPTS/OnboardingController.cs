using UnityEngine;
using System.Collections;
namespace Amused.XR
{
    /// <summary>
    /// Controls the onboarding sequence, guiding the player through the introduction and interactions.
    /// </summary>
    public class OnboardingController : MonoBehaviour
    {
        [Header("NPC Instructor Reference")]
        [SerializeField] private NPCInstructorController instructorNPC;
        [SerializeField] private OnboardingStepsHandler stepsHandler;

        private int currentStep = 0;
        private bool onboardingCompleted = false;

        private void Start()
        {
            Debug.Log("[OnboardingController] Start()");

            if (stepsHandler == null || instructorNPC == null)
            {
                Debug.LogError("[OnboardingController] Missing required references.");
                return;
            }

            stepsHandler.Initialize(instructorNPC, this);
            LoadOnboardingProgress();

            StartCoroutine(WaitForAudioLoadThenStart());
        }

        private IEnumerator WaitForAudioLoadThenStart()
        {
            while (!instructorNPC.IsAudioReady())
            {
                Debug.Log("[OnboardingController] Waiting for audio clips to load...");
                yield return null;
            }

            Debug.Log("[OnboardingController] Audio ready. Starting onboarding.");
            StartOnboarding();
        }

        public void StartOnboarding()
        {
            if (onboardingCompleted)
            {
                Debug.Log("[OnboardingController] Onboarding already completed. Skipping...");
                return;
            }

            Debug.Log("[OnboardingController] Starting onboarding process.");
            currentStep = 0;
            ProceedToNextStep();
        }

        public void ProceedToNextStep()
        {
            // Debug.LogWarning($"[ProceedToNextStep] CALLED at step {currentStep} — STACK:\n" + System.Environment.StackTrace);
            while (currentStep == 18 || currentStep == 19)
            {
                currentStep++;
            }

            stepsHandler.ExecuteStep(currentStep);
            currentStep++;
            SaveOnboardingProgress();
        }

        public void ResetOnboarding()
        {
            PlayerPrefs.DeleteKey("OnboardingStep");
            PlayerPrefs.DeleteKey("OnboardingCompleted");
            PlayerPrefs.Save();

            currentStep = 0;
            onboardingCompleted = false;

            Debug.Log("[OnboardingController] Onboarding reset.");
        }

        private void SaveOnboardingProgress()
        {
            PlayerPrefs.SetInt("OnboardingStep", currentStep);
            PlayerPrefs.SetInt("OnboardingCompleted", onboardingCompleted ? 1 : 0);
            PlayerPrefs.Save();

            Debug.Log($"[OnboardingController] Saved progress: Step {currentStep}, Completed: {onboardingCompleted}");
        }

        private void LoadOnboardingProgress()
        {
            currentStep = PlayerPrefs.GetInt("OnboardingStep", 0);
            onboardingCompleted = PlayerPrefs.GetInt("OnboardingCompleted", 0) == 1;

            Debug.Log($"[OnboardingController] Loaded progress: Step {currentStep}, Completed: {onboardingCompleted}");
        }

        private void CompleteOnboarding()
        {
            onboardingCompleted = true;
            SaveOnboardingProgress();
            Debug.Log("[OnboardingController] Onboarding completed. Transitioning to next phase.");
        }

        public int GetCurrentStep()
        {
            return currentStep;
        }

        public void SetStep(int step)
        {
            currentStep = step;
        }
    }
}



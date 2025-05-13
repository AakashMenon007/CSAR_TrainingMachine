using Amused.XR;
using System.Collections.Generic;
using UnityEngine;

namespace Amused.XR
{
    /// <summary>
    /// Handles execution of each step in the onboarding process.
    /// Keeps OnboardingController clean and modular.
    /// </summary>
    public class OnboardingStepsHandler : MonoBehaviour
    {
        private NPCInstructorController instructorNPC;
        private OnboardingController onboardingController;

        [Header("Onboarding Objects")]
        [SerializeField] private GameObject movementTriggerZone;
        [SerializeField] private GameObject grabbableObject;
        [SerializeField] private GameObject leverObject;
        [SerializeField] private GameObject valveObject;

        private readonly Dictionary<int, bool> autoProceedSteps = new Dictionary<int, bool>
        {
            { 0, true },   // 1.1 Intro
            { 1, true },   // 1.2 Panel info
            { 2, false },  // 1.3 UI button press
            { 3, true },   // 2.1 Movement explanation
            { 4, false },  // 2.2 Collider check
            { 5, true },   // 3.1 Grip intro
            { 6, false },  // 3.2 Grab check
            { 7, false },  // 3.3 Lever check
            { 8, false },  // 3.4 Valve check
            { 9, true },   // 3.5 Ready for CSAR
            { 10, true },  // 4.1 Scenario explained
            { 11, false }, // 4.2 Choice (button press)
            { 12, true },  // 4.4 Yes pressed
            { 13, true }   // 4.3 No pressed (restart)
        };

        public void Initialize(NPCInstructorController npcController, OnboardingController controller)
        {
            instructorNPC = npcController;
            onboardingController = controller;
            Debug.Log($"[OnboardingStepsHandler] Initialized.");
        }

        public void ExecuteStep(int step)
        {
            Debug.Log($"[OnboardingStepsHandler] Executing step {step}");

            bool shouldAutoProceed = autoProceedSteps.ContainsKey(step) && autoProceedSteps[step];

            switch (step)
            {
                case 0:
                    instructorNPC.PlayDialogue("onboarding_1a", shouldAutoProceed);
                    break;
                case 1:
                    instructorNPC.PlayDialogue("onboarding_1b", shouldAutoProceed);
                    break;
                case 2:
                    instructorNPC.PlayDialogue("onboarding_1c", shouldAutoProceed);
                    // Waits for UI button press
                    break;
                case 3:
                    instructorNPC.PlayDialogue("onboarding_2a", shouldAutoProceed);
                    break;
                case 4:
                    instructorNPC.PlayDialogue("onboarding_2b", shouldAutoProceed);
                    movementTriggerZone.SetActive(true); // Enable collider trigger
                    break;
                case 5:
                    instructorNPC.PlayDialogue("onboarding_3a", shouldAutoProceed);
                    break;
                case 6:
                    instructorNPC.PlayDialogue("onboarding_3b", shouldAutoProceed);
                    grabbableObject.SetActive(true);
                    break;
                case 7:
                    instructorNPC.PlayDialogue("onboarding_3c", shouldAutoProceed);
                    leverObject.SetActive(true);
                    break;
                case 8:
                    instructorNPC.PlayDialogue("onboarding_3d", shouldAutoProceed);
                    valveObject.SetActive(true);
                    break;
                case 9:
                    instructorNPC.PlayDialogue("onboarding_3e", shouldAutoProceed);
                    break;
                case 10:
                    instructorNPC.PlayDialogue("onboarding_4a", shouldAutoProceed);
                    break;
                case 11:
                    instructorNPC.PlayDialogue("onboarding_4b", shouldAutoProceed);
                    // Awaits button choice
                    break;
                case 12:
                    instructorNPC.PlayDialogue("onboarding_4b_yes", shouldAutoProceed);
                    //onboardingController.GoToNextStage(); // Example method
                    break;
                case 13:
                    instructorNPC.PlayDialogue("onboarding_4b_no", shouldAutoProceed);
                    onboardingController.ResetOnboarding();
                    break;
                default:
                    Debug.LogWarning($"[OnboardingStepsHandler] Invalid step {step}");
                    break;
            }
        }

        private void SaveProgress()
        {
            Debug.Log("[OnboardingStepsHandler] Simulated progress save.");
        }
    }
}

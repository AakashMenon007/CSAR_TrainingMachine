using System.Collections.Generic;
using UnityEngine;

namespace Amused.XR
{
    public class OnboardingStepsHandler : MonoBehaviour
    {
        private NPCInstructorController instructorNPC;
        private OnboardingController onboardingController;

        [Header("Onboarding Objects")]
        [SerializeField] private GameObject movementTriggerZone;
        [SerializeField] private GameObject grabbableObject;
        [SerializeField] private GameObject leverObject;
        [SerializeField] private GameObject valveObject;
        [SerializeField] private GameObject physicalButton;

        private readonly Dictionary<int, bool> autoProceedSteps = new Dictionary<int, bool>
        {
            { 0, true },   // onboarding_1a
            { 1, true },   // onboarding_1b
            { 2, true },   // onboarding_2a
            { 3, false },  // onboarding_2b (collider check)
            { 4, false },  // onboarding_2c (button check)
            { 5, true },   // onboarding_3a
            { 6, false },  // onboarding_3b (grab check)
            { 7, false },  // onboarding_3c (lever check)
            { 8, false },  // onboarding_3d (valve check)
            { 9, true },   // onboarding_3e
            { 10, true },  // onboarding_4a
            { 11, false }, // onboarding_4b (button check)
            { 12, true },  // onboarding_4b_no
            { 13, true }   // onboarding_4b_yes
        };

        public void Initialize(NPCInstructorController npcController, OnboardingController controller)
        {
            instructorNPC = npcController;
            onboardingController = controller;
        }

        public void ExecuteStep(int step)
        {
            bool shouldAutoProceed = autoProceedSteps.TryGetValue(step, out var auto) && auto;

            switch (step)
            {
                case 0:
                    instructorNPC.PlayDialogue("onboarding_1a", shouldAutoProceed);
                    break;
                case 1:
                    instructorNPC.PlayDialogue("onboarding_1b", shouldAutoProceed);
                    break;
                case 2:
                    instructorNPC.PlayDialogue("onboarding_2a", shouldAutoProceed);
                    break;
                case 3:
                    instructorNPC.PlayDialogue("onboarding_2b", shouldAutoProceed);
                    movementTriggerZone.SetActive(true);
                    break;
                case 4:
                    instructorNPC.PlayDialogue("onboarding_2c", shouldAutoProceed);
                    ActivateButton();
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
                    ActivateButton();
                    break;
                case 12:
                    instructorNPC.PlayDialogue("onboarding_4b_no", shouldAutoProceed);
                    onboardingController.ResetOnboarding();
                    break;
                case 13:
                    instructorNPC.PlayDialogue("onboarding_4b_yes", shouldAutoProceed);
                    break;
                default:
                    Debug.LogWarning($"[OnboardingStepsHandler] Invalid step {step}");
                    break;
            }
        }

        private void ActivateButton()
        {
            if (physicalButton != null)
            {
                physicalButton.SetActive(true);
            }
        }
    }
}

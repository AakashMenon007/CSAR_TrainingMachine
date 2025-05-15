using System.Collections;
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
        [SerializeField] private GameObject yesButton;
        [SerializeField] private GameObject noButton;

        [Header("OSH Objects")]
        [SerializeField] private GameObject coatObject;
        [SerializeField] private GameObject leftGlove;
        [SerializeField] private GameObject rightGlove;

        private readonly Dictionary<int, bool> autoProceedSteps = new Dictionary<int, bool>
        {
            { 0, true },   // onboarding_1a
            { 1, true },   // onboarding_1b
            { 2, true },   // onboarding_2a
            { 3, false },  // onboarding_2b
            { 4, false },  // onboarding_2c
            { 5, true },   // onboarding_3a
            { 6, false },  // onboarding_3b
            { 7, false },  // onboarding_3c
            { 8, false },  // onboarding_3d
            { 9, true },   // onboarding_3e
            {10, true },   // onboarding_4a
            {11, false },  // onboarding_4b
            {12, true },   // onboarding_4b_no
            {13, true }    // onboarding_4b_yes
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
                    yesButton.SetActive(true);
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
                    ActivateButtons();
                    break;
                case 12:
                    instructorNPC.PlayDialogue("onboarding_4b_no", shouldAutoProceed);
                    StartCoroutine(ResetAfterDialogue());
                    break;
                case 13:
                    instructorNPC.PlayDialogue("onboarding_4b_yes", shouldAutoProceed); // for some reason this case calls case 12 which restarts the onboarding so I will just skip it for now
                    break;
                case 14:
                    instructorNPC.PlayDialogue("osh_1a", shouldAutoProceed);
                    break;
                case 15:
                    instructorNPC.PlayDialogue("osh_1b_coat", shouldAutoProceed);
                    if (coatObject != null) coatObject.SetActive(true);
                    StartCoroutine(EnableCoatWearColliderAfterDialogue());
                    break;
                case 16:
                    instructorNPC.PlayDialogue("osh_1b_waiting", shouldAutoProceed);
                    break;
                case 17:
                    instructorNPC.PlayDialogue("osh_1b_warning", shouldAutoProceed);
                    break;
                case 18:
                    instructorNPC.PlayDialogue("osh_1b_gloves", shouldAutoProceed);
                    if (leftGlove != null) leftGlove.SetActive(true);
                    if (rightGlove != null) rightGlove.SetActive(true);
                    break;
                case 19:
                    instructorNPC.PlayDialogue("osh_1c", shouldAutoProceed);
                    StartCoroutine(SwitchSceneAfterDialogue()); // if you're transitioning
                    break;
                default:
                    Debug.LogWarning($"[OnboardingStepsHandler] Invalid step {step}");
                    break;
            }
        }

        private void ActivateButtons()
        {
            if (yesButton != null) yesButton.SetActive(true);
            if (noButton != null) noButton.SetActive(true);
        }

        private IEnumerator ResetAfterDialogue()
        {
            yield return new WaitUntil(() => !instructorNPC.DialogueIsActive);
            onboardingController.ResetOnboarding();
        }

        private IEnumerator SwitchSceneAfterDialogue()
        {
            yield return new WaitUntil(() => !instructorNPC.DialogueIsActive);
            // SceneManager.LoadScene("NextSceneName");
        }

        [SerializeField] private Collider playerCoatTrigger;
        private IEnumerator EnableCoatWearColliderAfterDialogue()
        {
            yield return new WaitUntil(() => !instructorNPC.DialogueIsActive);
            if (playerCoatTrigger != null) playerCoatTrigger.enabled = true;
        }

    }
}

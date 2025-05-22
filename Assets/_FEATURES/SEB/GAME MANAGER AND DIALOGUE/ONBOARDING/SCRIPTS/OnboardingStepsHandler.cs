using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField] private GameObject dialObject;
        [SerializeField] private GameObject sliderObject;
        [SerializeField] private GameObject yesButton;
        [SerializeField] private GameObject noButton;

        [Header("OSH Objects")]
        [SerializeField] private GameObject coatObject;
        [SerializeField] private GameObject leftGlove;
        [SerializeField] private GameObject rightGlove;

        [Header("Trigger Scripts (for reset)")]
        [SerializeField] private OnboardingRotatableTrigger valveTrigger;
        [SerializeField] private OnboardingRotatableTrigger dialTrigger;
        [SerializeField] private OnboardingSliderTrigger sliderTrigger;

        [SerializeField] private Collider playerCoatTrigger;

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
            { 8, false },  // onboarding_3d (valve)
            { 9, false },  // onboarding_3e (dial)
            {10, false },  // onboarding_3f (slider)
            {11, true },   // onboarding_3g
            {12, true },   // onboarding_4a
            {13, false },  // onboarding_4b
            {14, true },   // onboarding_4b_no
            {15, true },   // onboarding_4b_yes
        };

        public void Initialize(NPCInstructorController npcController, OnboardingController controller)
        {
            instructorNPC = npcController;
            onboardingController = controller;
        }

        public void ExecuteStep(int step)
        {
            bool shouldAutoProceed = autoProceedSteps.TryGetValue(step, out var auto) && auto;

            DeactivateAllOnboardingObjects();

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
                    if (movementTriggerZone != null) movementTriggerZone.SetActive(true);
                    break;
                case 4:
                    instructorNPC.PlayDialogue("onboarding_2c", shouldAutoProceed);
                    if (yesButton != null) yesButton.SetActive(true);
                    break;
                case 5:
                    instructorNPC.PlayDialogue("onboarding_3a", shouldAutoProceed);
                    break;
                case 6:
                    instructorNPC.PlayDialogue("onboarding_3b", shouldAutoProceed);
                    if (grabbableObject != null) grabbableObject.SetActive(true);
                    break;
                case 7:
                    instructorNPC.PlayDialogue("onboarding_3c", shouldAutoProceed);
                    if (leverObject != null) leverObject.SetActive(true);
                    break;
                case 8:
                    instructorNPC.PlayDialogue("onboarding_3d", shouldAutoProceed); // Valve
                    if (valveObject != null) valveObject.SetActive(true);
                    break;
                case 9:
                    instructorNPC.PlayDialogue("onboarding_3e", shouldAutoProceed); // Dial
                    if (dialObject != null) dialObject.SetActive(true);
                    break;
                case 10:
                    instructorNPC.PlayDialogue("onboarding_3f", shouldAutoProceed); // Slider
                    if (sliderObject != null) sliderObject.SetActive(true);
                    break;
                case 11:
                    instructorNPC.PlayDialogue("onboarding_3g", shouldAutoProceed);
                    break;
                case 12:
                    instructorNPC.PlayDialogue("onboarding_4a", shouldAutoProceed);
                    break;
                case 13:
                    instructorNPC.PlayDialogue("onboarding_4b", shouldAutoProceed);
                    ActivateButtons();
                    break;
                case 14:
                    instructorNPC.PlayDialogue("onboarding_4b_no", shouldAutoProceed);
                    StartCoroutine(ResetAfterDialogue());
                    break;
                case 15:
                    instructorNPC.PlayDialogue("onboarding_4b_yes", shouldAutoProceed);
                    break;
                case 16:
                    instructorNPC.PlayDialogue("osh_1a", shouldAutoProceed);
                    if (coatObject != null) coatObject.SetActive(true);
                    if (playerCoatTrigger != null) playerCoatTrigger.enabled = true;
                    break;
                case 17:
                    instructorNPC.PlayDialogue("osh_1b_coat", shouldAutoProceed);
                    break;
                case 18:
                    instructorNPC.PlayDialogue("osh_1b_waiting", shouldAutoProceed);
                    break;
                case 19:
                    instructorNPC.PlayDialogue("osh_1b_warning", shouldAutoProceed);
                    break;
                case 20:
                    instructorNPC.PlayDialogue("osh_1b_gloves", shouldAutoProceed);
                    if (leftGlove != null) leftGlove.SetActive(true);
                    if (rightGlove != null) rightGlove.SetActive(true);
                    break;
                case 21:
                    instructorNPC.PlayDialogue("osh_1c", shouldAutoProceed);
                    StartCoroutine(SwitchSceneAfterDialogue());
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

        private void DeactivateAllOnboardingObjects()
        {
            if (movementTriggerZone != null) movementTriggerZone.SetActive(false);
            if (grabbableObject != null) grabbableObject.SetActive(false);
            if (leverObject != null) leverObject.SetActive(false);
            if (valveObject != null) valveObject.SetActive(false);
            if (dialObject != null) dialObject.SetActive(false);
            if (sliderObject != null) sliderObject.SetActive(false);
            if (yesButton != null) yesButton.SetActive(false);
            if (noButton != null) noButton.SetActive(false);
            // Deactivate any OSH objects here if needed
        }

        private IEnumerator ResetAfterDialogue()
        {
            yield return new WaitUntil(() => !instructorNPC.DialogueIsActive);
            ResetAllTriggers();
            onboardingController.ResetOnboarding();
        }

        // Reset all triggers after onboarding reset
        private void ResetAllTriggers()
        {
            if (valveTrigger != null) valveTrigger.ResetTrigger();
            if (dialTrigger != null) dialTrigger.ResetTrigger();
            if (sliderTrigger != null) sliderTrigger.ResetTrigger();
            // Reset other triggers if you add more
        }

        private IEnumerator SwitchSceneAfterDialogue()
        {
            yield return new WaitUntil(() => !instructorNPC.DialogueIsActive);
            SceneManager.LoadScene("2.KnowledgeBuilding");
        }

        private IEnumerator EnableCoatWearColliderAfterDialogue()
        {
            yield return new WaitUntil(() => !instructorNPC.DialogueIsActive);
            if (playerCoatTrigger != null) playerCoatTrigger.enabled = true;
        }
    }
}

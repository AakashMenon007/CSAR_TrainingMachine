using UnityEngine;

namespace Amused.XR
{
    /// <summary>
    /// Detects when the player enters the onboarding trigger area.
    /// </summary>
    public class OnboardingTrigger : MonoBehaviour
    {
        private OnboardingController onboardingController;
        private NPCInstructorController npc;

        private void Start()
        {
            onboardingController = FindObjectOfType<OnboardingController>();
            npc = FindObjectOfType<NPCInstructorController>();

            if (onboardingController == null)
                Debug.LogError("[OnboardingTrigger] OnboardingController not found in the scene!");

            if (npc == null)
                Debug.LogError("[OnboardingTrigger] NPCInstructorController not found in the scene!");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (npc != null && npc.DialogueIsActive)
            {
                Debug.Log("[OnboardingTrigger] Entered too early — dialogue not finished.");
                return;
            }

            Debug.Log("[OnboardingTrigger] Player entered — proceeding to next step.");
            onboardingController?.ProceedToNextStep();
            gameObject.SetActive(false);
        }
    }
}

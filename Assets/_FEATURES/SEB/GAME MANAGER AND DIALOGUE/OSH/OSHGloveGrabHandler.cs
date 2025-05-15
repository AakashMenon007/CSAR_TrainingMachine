using UnityEngine;
using System.Collections;

namespace Amused.XR
{
    public class OSHGloveEquipTrigger : MonoBehaviour
    {
        [Tooltip("Reference to the corresponding hand collider's tag (e.g., 'LeftHandGloveZone' or 'RightHandGloveZone')")]
        public string handZoneTag;

        [Tooltip("Material to apply to the hand (optional)")]
        public Renderer handRenderer;
        public Material equippedHandMaterial;

        [Tooltip("Audio to play when glove is equipped")]
        public AudioSource equipSound;

        [Tooltip("Set to true if this is the LEFT glove, false for RIGHT")]
        public bool isLeftGlove;

        public OnboardingController onboardingController;

        private static bool leftGloveEquipped = false;
        private static bool rightGloveEquipped = false;
        private bool isEquipped = false;

        private void OnTriggerEnter(Collider other)
        {
            if (isEquipped) return;
            if (!other.CompareTag(handZoneTag)) return;

            isEquipped = true;
            StartCoroutine(EquipGloveRoutine());
        }

        private IEnumerator EquipGloveRoutine()
        {
            // Play sound instantly
            if (equipSound != null)
                equipSound.Play();

            // Change hand material immediately (if needed)
            if (handRenderer != null && equippedHandMaterial != null)
                handRenderer.material = equippedHandMaterial;

            // Mark as equipped
            if (isLeftGlove)
                leftGloveEquipped = true;
            else
                rightGloveEquipped = true;

            Debug.Log($"[OSHGloveEquipTrigger] {(isLeftGlove ? "Left" : "Right")} glove equipped.");

            // Wait for sound to finish (or 0.5s if no sound)
            float waitTime = (equipSound != null && equipSound.clip != null) ? equipSound.clip.length : 0.5f;
            yield return new WaitForSeconds(waitTime);

            // Deactivate glove object
            gameObject.SetActive(false);

            // Check if both gloves are now equipped
            if (leftGloveEquipped && rightGloveEquipped && onboardingController != null)
            {
                Debug.Log("[OSHGloveEquipTrigger] Both gloves equipped — proceeding.");
                onboardingController.ProceedToNextStep();
            }
        }
    }
}

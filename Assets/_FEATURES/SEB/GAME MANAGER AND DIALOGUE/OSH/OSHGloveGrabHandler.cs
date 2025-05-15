using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

namespace Amused.XR
{
    public class GloveGrabHandler : MonoBehaviour
    {
        [Tooltip("Reference to the corresponding hand object (e.g., left or right)")]
        public Renderer handRenderer;

        [Tooltip("Material to apply once glove is equipped")]
        public Material equippedHandMaterial;

        [Tooltip("Audio to play when glove is equipped")]
        public AudioSource equipSound;

        [Tooltip("Set to true if this is the LEFT glove, false for RIGHT")]
        public bool isLeftGlove;

        private static bool leftGloveEquipped = false;
        private static bool rightGloveEquipped = false;

        private bool hasBeenGrabbed = false;

        public OnboardingController onboardingController;
        public int validStep = 18;

        public void OnGrab(SelectEnterEventArgs args)
        {
            if (hasBeenGrabbed || onboardingController.GetCurrentStep() != validStep)
                return;

            hasBeenGrabbed = true;
            StartCoroutine(EquipGloveAfterDelay());
        }

        private IEnumerator EquipGloveAfterDelay()
        {
            yield return new WaitForSeconds(1f);

            // Play sound
            if (equipSound != null)
                equipSound.Play();

            // Change hand material
            if (handRenderer != null && equippedHandMaterial != null)
                handRenderer.material = equippedHandMaterial;

            // Update static state
            if (isLeftGlove)
                leftGloveEquipped = true;
            else
                rightGloveEquipped = true;

            Debug.Log($"[GloveGrabHandler] {(isLeftGlove ? "Left" : "Right")} glove equipped.");

            // Deactivate the glove object
            gameObject.SetActive(false);

            // Check if both are done
            if (leftGloveEquipped && rightGloveEquipped)
            {
                Debug.Log("[GloveGrabHandler] Both gloves equipped — proceeding.");
                onboardingController.ProceedToNextStep();
            }
        }
    }
}


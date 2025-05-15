using UnityEngine;
using System.Collections;

namespace Amused.XR
{
    public class CoatWearTrigger : MonoBehaviour
    {
        public AudioSource equipSound;
        public float soundDelay = 0.2f;
        public OnboardingController onboardingController;

        private bool isEquipped = false;

        private void OnTriggerEnter(Collider other)
        {
            if (isEquipped) return;

            if (other.CompareTag("PlayerCoatZone"))
            {
                isEquipped = true;
                Debug.Log("[CoatWearTrigger] Coat equipped trigger!");
                if (equipSound != null) equipSound.Play();
                float delay = equipSound != null ? equipSound.clip.length + soundDelay : 0.5f;
                StartCoroutine(DeactivateAfterSound(delay));
            }
        }

        private IEnumerator DeactivateAfterSound(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
            if (onboardingController != null)
            {
                onboardingController.SetStep(18);           // Skip to gloves step
                onboardingController.ProceedToNextStep();   // Play gloves logic
            }
        }
    }
}

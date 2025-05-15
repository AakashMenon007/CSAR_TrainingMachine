using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

namespace Amused.XR
{
    public class CoatGrabHandler : MonoBehaviour
    {
        [Tooltip("Audio to play when coat is equipped")]
        public AudioSource equipSound;

        public OnboardingController onboardingController;
        public int validStep = 15;

        private bool hasBeenGrabbed = false;

        public void OnGrab(SelectEnterEventArgs args)
        {
            if (hasBeenGrabbed || onboardingController.GetCurrentStep() != validStep)
                return;

            hasBeenGrabbed = true;
            StartCoroutine(EquipCoatAfterDelay());
        }

        private IEnumerator EquipCoatAfterDelay()
        {
            yield return new WaitForSeconds(1f);

            if (equipSound != null)
                equipSound.Play();

            Debug.Log("[CoatGrabHandler] Coat equipped — hiding object and proceeding.");
            gameObject.SetActive(false);

            onboardingController.ProceedToNextStep();
        }
    }
}

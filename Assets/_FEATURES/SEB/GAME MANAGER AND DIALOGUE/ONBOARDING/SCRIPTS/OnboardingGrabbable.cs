using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Amused.XR
{
    public class OnboardingGrabbable : MonoBehaviour
    {
        public OnboardingController onboardingController;
        public float delayBeforeProceeding = 3f;

        private XRGrabInteractable grab;
        private bool hasTriggered = false;

        private void Awake()
        {
            grab = GetComponent<XRGrabInteractable>();
        }

        private void OnEnable()
        {
            if (grab != null)
            {
                grab.selectEntered.AddListener(OnGrabbed);
                grab.selectExited.AddListener(OnReleased);
            }
        }

        private void OnDisable()
        {
            if (grab != null)
            {
                grab.selectEntered.RemoveListener(OnGrabbed);
                grab.selectExited.RemoveListener(OnReleased);
            }
        }

        private void OnGrabbed(SelectEnterEventArgs args)
        {
            if (hasTriggered) return;

            hasTriggered = true;
            Debug.Log("[OnboardingGrabbable] Grabbed — starting coroutine.");
            StartCoroutine(WaitThenProceed());
        }

        private void OnReleased(SelectExitEventArgs args)
        {
            Debug.Log("[OnboardingGrabbable] Released — hiding object.");
            gameObject.SetActive(false);
        }

        private IEnumerator WaitThenProceed()
        {
            yield return new WaitForSeconds(delayBeforeProceeding);
            onboardingController?.ProceedToNextStep();
        }
    }
}

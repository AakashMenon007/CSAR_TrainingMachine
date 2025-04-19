using System.Collections;
using UnityEngine;
using Oculus.Interaction; // Meta Interaction SDK

namespace Amused.XR
{
    /// <summary>
    /// Detects when a player grabs an object and proceeds to the next onboarding step after a delay.
    /// </summary>
    public class OnboardingGrabbable : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float grabHoldDelay = 3f; // Adjustable delay before proceeding

        private OnboardingController onboardingController;
        private Grabbable grabbable; // MRUK Grabbable Component

        private void Start()
        {
            onboardingController = FindObjectOfType<OnboardingController>();
            if (onboardingController == null)
            {
                Debug.LogError("[OnboardingGrabbable] OnboardingController not found in the scene!");
            }

            grabbable = GetComponent<Grabbable>();
            if (grabbable == null)
            {
                Debug.LogError("[OnboardingGrabbable] No Grabbable component found on this object!");
            }
            else
            {
                grabbable.WhenPointerEventRaised += OnGrab;
            }
        }

        private void OnDestroy()
        {
            if (grabbable != null)
            {
                grabbable.WhenPointerEventRaised -= OnGrab;
            }
        }

        /// <summary>
        /// Called when the object is grabbed.
        /// </summary>
        private void OnGrab(PointerEvent evt)
        {
            if (evt.Type == PointerEventType.Select) // Object grabbed
            {
                Debug.Log("[OnboardingGrabbable] Object grabbed! Waiting before proceeding...");
                StartCoroutine(ProceedAfterDelay());
                grabbable.WhenPointerEventRaised -= OnGrab; // Prevent multiple triggers
            }
        }

        /// <summary>
        /// Waits for the set delay, then proceeds to the next onboarding step.
        /// </summary>
        private IEnumerator ProceedAfterDelay()
        {
            yield return new WaitForSeconds(grabHoldDelay);
            Debug.Log("[OnboardingGrabbable] Grab hold delay over. Proceeding to next step.");
            onboardingController.ProceedToNextStep();
        }
    }
}

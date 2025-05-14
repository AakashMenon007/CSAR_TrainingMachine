using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Amused.XR
{
    public class GrabbableTrigger : MonoBehaviour
    {
        public OnboardingController onboardingController;
        private NPCInstructorController npc;
        private XRGrabInteractable grab;

        private bool hasProceeded = false;
        private bool isHeld = false;

        private void Awake()
        {
            grab = GetComponent<XRGrabInteractable>();
            npc = FindObjectOfType<NPCInstructorController>();
        }

        private void OnEnable()
        {
            if (grab != null)
            {
                grab.selectEntered.AddListener(OnGrabbed);
                grab.selectExited.AddListener(OnReleased);
            }

            hasProceeded = false;
            isHeld = false;
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
            if (hasProceeded)
                return;

            if (npc != null && npc.DialogueIsActive)
            {
                Debug.Log("[GrabbableTrigger] Ignored grab — dialogue still active.");
                return;
            }

            Debug.Log("[GrabbableTrigger] Grab accepted — advancing step.");
            onboardingController?.ProceedToNextStep();
            hasProceeded = true;
            isHeld = true;
        }

        private void OnReleased(SelectExitEventArgs args)
        {
            if (hasProceeded && isHeld)
            {
                Debug.Log("[GrabbableTrigger] Released after step — hiding object.");
                gameObject.SetActive(false);
            }
        }
    }
}

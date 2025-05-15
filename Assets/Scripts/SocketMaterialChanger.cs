using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketMaterialChanger : MonoBehaviour
{
    [Header("Socket Interactor")]
    public XRSocketInteractor socketInteractor;

    [Header("Target Object")]
    public GameObject targetObject;

    [Header("Materials")]
    public Material defaultMaterial;
    public Material newMaterial;

    private Renderer targetRenderer;

    private void Start()
    {
        // Ensure the socketInteractor and targetObject are assigned
        if (socketInteractor == null || targetObject == null)
        {
            Debug.LogError("SocketInteractor or TargetObject not assigned.");
            return;
        }

        // Get the renderer of the target object
        targetRenderer = targetObject.GetComponent<Renderer>();
        if (targetRenderer == null)
        {
            Debug.LogError("TargetObject does not have a Renderer component.");
            return;
        }

        // Set the default material initially
        targetRenderer.material = defaultMaterial;

        // Subscribe to socket events
        socketInteractor.selectEntered.AddListener(OnObjectAttached);
        socketInteractor.selectExited.AddListener(OnObjectDetached);
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnObjectAttached);
            socketInteractor.selectExited.RemoveListener(OnObjectDetached);
        }
    }

    private void OnObjectAttached(SelectEnterEventArgs args)
    {
        // Change the material to the new material
        if (targetRenderer != null && newMaterial != null)
        {
            targetRenderer.material = newMaterial;
        }
    }

    private void OnObjectDetached(SelectExitEventArgs args)
    {
        // Revert the material to the default material
        if (targetRenderer != null && defaultMaterial != null)
        {
            targetRenderer.material = defaultMaterial;
        }
    }
}

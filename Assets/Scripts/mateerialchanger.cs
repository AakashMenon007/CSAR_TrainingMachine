using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [Header("Target Game Object")]
    [Tooltip("The game object whose material will be changed.")]
    public GameObject targetObject;

    [Header("Materials")]
    [Tooltip("The list of materials to cycle through.")]
    public Material[] materials;

    [Header("Time Settings")]
    [Tooltip("Delay time between material changes (in seconds).")]
    public float delayTime = 2.0f;

    private Renderer targetRenderer;
    private int currentMaterialIndex = 0;

    private void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object is not assigned!");
            return;
        }

        targetRenderer = targetObject.GetComponent<Renderer>();

        if (targetRenderer == null)
        {
            Debug.LogError("Target Object does not have a Renderer component!");
            return;
        }

        if (materials.Length == 0)
        {
            Debug.LogError("No materials assigned in the Materials array!");
            return;
        }

        // Start the material change loop
        InvokeRepeating(nameof(ChangeMaterial), delayTime, delayTime);
    }

    private void ChangeMaterial()
    {
        if (materials.Length == 0 || targetRenderer == null) return;

        // Cycle through materials
        targetRenderer.material = materials[currentMaterialIndex];
        currentMaterialIndex = (currentMaterialIndex + 1) % materials.Length;
    }

    private void OnDisable()
    {
        // Stop the repeating invoke when the object is disabled
        CancelInvoke(nameof(ChangeMaterial));
    }
}

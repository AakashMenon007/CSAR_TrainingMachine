using UnityEngine;
using System.Collections;

public class TankSanitizationEffect : MonoBehaviour
{
    public Collider sanitizingToolCollider;    // Assign water can's collider
    public Material sanitizedMaterial;         // Assign clear material
    public float sanitizationDelay = 1.0f;     // Time until material changes

    private Material originalTankMaterial;
    private Renderer tankRenderer;
    private bool isSanitizing;

    void Start()
    {
        tankRenderer = GetComponent<Renderer>();
        if (tankRenderer != null)
        {
            originalTankMaterial = tankRenderer.material;
            Debug.Log("Original tank material stored.");
        }
        else
        {
            Debug.LogWarning("No Renderer found on tank GameObject.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.name}");
        if (other == sanitizingToolCollider && !isSanitizing)
        {
            Debug.Log("Sanitization tool entered tank zone");
            StartCoroutine(ChangeTankMaterialAfterDelay());
        }
    }

    IEnumerator ChangeTankMaterialAfterDelay()
    {
        isSanitizing = true;
        Debug.Log($"Sanitization started. Waiting {sanitizationDelay} seconds...");
        yield return new WaitForSeconds(sanitizationDelay);

        if (tankRenderer != null && sanitizedMaterial != null)
        {
            tankRenderer.material = sanitizedMaterial;
            Debug.Log("Tank material sanitized!");
        }
        else
        {
            Debug.LogWarning("Missing components for material change");
        }
        isSanitizing = false;
    }

    public void ResetTankMaterial()
    {
        if (tankRenderer != null)
        {
            tankRenderer.material = originalTankMaterial;
            Debug.Log("Tank material reset to original");
        }
    }
}

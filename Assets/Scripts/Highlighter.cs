using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public Color highlightColor = Color.yellow;

    private List<Material> originalMaterials = new List<Material>();
    private List<Material> highlightMaterials = new List<Material>();
    private List<Renderer> renderers = new List<Renderer>();

    void Awake()
    {
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        foreach (Renderer rend in renderers)
        {
            Material original = rend.material;
            Material highlight = new Material(original);

            // For URP Lit shader: enable emission
            if (highlight.HasProperty("_EmissionColor"))
            {
                highlight.SetColor("_EmissionColor", highlightColor);
                highlight.EnableKeyword("_EMISSION");
            }

            originalMaterials.Add(original);
            highlightMaterials.Add(highlight);
        }
    }

    public void Highlight()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material = highlightMaterials[i];
        }
    }

    public void RemoveHighlight()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }
}

using UnityEngine;
using DG.Tweening;

public class MultiRendererHighlighter : MonoBehaviour
{
    [Header("Emission Settings")]
    [SerializeField] private Color emissionColor = Color.white;
    [SerializeField] private float maxEmissionIntensity = 2.0f;
    [SerializeField] private float minEmissionIntensity = 0.0f;

    [Header("Animation Settings")]
    [SerializeField] private float pulseDuration = 1.0f;
    [SerializeField] private Ease pulseEaseType = Ease.InOutSine;

    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");
    private static readonly string EmissionKeyword = "_EMISSION";

    private Renderer[] renderers;
    private Material[] materials;
    private Sequence[] sequences;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
        materials = new Material[renderers.Length];
        sequences = new Sequence[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
            materials[i].EnableKeyword(EmissionKeyword);
            SetEmissionIntensity(materials[i], minEmissionIntensity);
        }
    }

    public void Highlight()
    {
        StopHighlight();

        for (int i = 0; i < materials.Length; i++)
        {
            int index = i;
            sequences[i] = DOTween.Sequence();
            sequences[i].Append(
                DOTween.To(
                    () => minEmissionIntensity,
                    val => SetEmissionIntensity(materials[index], val),
                    maxEmissionIntensity,
                    pulseDuration / 2f
                ).SetEase(pulseEaseType)
            ).Append(
                DOTween.To(
                    () => maxEmissionIntensity,
                    val => SetEmissionIntensity(materials[index], val),
                    minEmissionIntensity,
                    pulseDuration / 2f
                ).SetEase(pulseEaseType)
            ).SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void StopHighlight()
    {
        for (int i = 0; i < sequences.Length; i++)
        {
            sequences[i]?.Kill();
            if (materials[i] != null)
            {
                SetEmissionIntensity(materials[i], minEmissionIntensity);
            }
        }
    }

    private void SetEmissionIntensity(Material mat, float intensity)
    {
        mat.SetColor(EmissionColorID, emissionColor * intensity);
    }
}

using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Renderer))]
public class MaterialEmissionAnimator : MonoBehaviour
{
    [Header("Emission Settings")]
    [SerializeField] private Color emissionColor = Color.white;
    [SerializeField] private float maxEmissionIntensity = 2.0f;
    [SerializeField] private float minEmissionIntensity = 0.0f;

    [Header("Animation Settings")]
    [SerializeField] private float pulseDuration = 1.0f;
    [SerializeField] private Ease pulseEaseType = Ease.InOutSine;
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private bool loopAnimation = true;
    [SerializeField] private int loopCount = -1; // -1 for infinite
    [SerializeField] private LoopType loopType = LoopType.Yoyo;

    // Material property references
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly string EmissionKeyword = "_EMISSION";

    // Component references
    private Renderer _renderer;
    private Material _material;
    private Sequence _animationSequence;

    private void Awake()
    {
        // Get the renderer component
        _renderer = GetComponent<Renderer>();

        // Get the material (creates an instance if shared)
        _material = _renderer.material;

        // Ensure emission is supported and enabled on the material
        SetupEmission(); // ✅ Use helper method to prepare material for emission

        // Set initial emission to minimum
        SetEmissionIntensity(minEmissionIntensity);

        // Start animation if set to play on awake
        if (playOnAwake)
        {
            StartEmissionAnimation();
        }
    }

    /// <summary>
    /// Ensures emission keyword is enabled and lighting flags are properly set for URP/Android
    /// </summary>
    private void SetupEmission()
    {
        if (_material.HasProperty(EmissionColor))
        {
            // ✅ Explicitly enable the emission keyword for build compatibility
            _material.EnableKeyword(EmissionKeyword);

            // ✅ Ensure global illumination flag is set for real-time emission support
            _material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        }
        else
        {
            Debug.LogWarning("Material does not support _EmissionColor: " + _material.name);
        }
    }

    private void OnDestroy()
    {
        // Clean up the DOTween sequence when object is destroyed
        _animationSequence?.Kill();

        // Clean up material instance if needed
#if UNITY_EDITOR
        if (_material != null && !Application.isPlaying)
        {
            DestroyImmediate(_material);
        }
        else if (_material != null)
        {
            Destroy(_material);
        }
#else
        if (_material != null)
        {
            Destroy(_material);
        }
#endif
    }

    /// <summary>
    /// Set the emission intensity of the material
    /// </summary>
    /// <param name="intensity">Emission intensity value</param>
    public void SetEmissionIntensity(float intensity)
    {
        if (_material != null)
        {
            // ✅ Apply gamma correction for URP and Android builds
            Color emissionWithIntensity = emissionColor * Mathf.LinearToGammaSpace(intensity);

            // Set the emission color property
            _material.SetColor(EmissionColor, emissionWithIntensity);
        }
    }

    /// <summary>
    /// Start the emission animation sequence
    /// </summary>
    public void StartEmissionAnimation()
    {
        // Kill any existing animation
        _animationSequence?.Kill();

        // Create a new DOTween sequence
        _animationSequence = DOTween.Sequence();

        // Add the emission intensity animation
        _animationSequence.Append(
            DOTween.To(
                () => minEmissionIntensity,
                SetEmissionIntensity,
                maxEmissionIntensity,
                pulseDuration / 2
            ).SetEase(pulseEaseType)
        );

        // Add the reverse animation if using Yoyo loop
        if (loopType == LoopType.Yoyo)
        {
            _animationSequence.Append(
                DOTween.To(
                    () => maxEmissionIntensity,
                    SetEmissionIntensity,
                    minEmissionIntensity,
                    pulseDuration / 2
                ).SetEase(pulseEaseType)
            );
        }

        // Configure looping
        if (loopAnimation)
        {
            _animationSequence.SetLoops(loopCount, loopType);
        }

        // Play the sequence
        _animationSequence.Play();
    }

    /// <summary>
    /// Stop the emission animation
    /// </summary>
    public void StopEmissionAnimation()
    {
        _animationSequence?.Kill();
        SetEmissionIntensity(minEmissionIntensity);
    }

    /// <summary>
    /// Highlight with maximum emission instantly (no animation)
    /// </summary>
    public void HighlightInstantly()
    {
        StopEmissionAnimation();
        SetEmissionIntensity(maxEmissionIntensity);
    }

    /// <summary>
    /// Change the emission color at runtime
    /// </summary>
    /// <param name="newColor">New emission color</param>
    public void ChangeEmissionColor(Color newColor)
    {
        emissionColor = newColor;

        // Update current emission with new color
        float currentIntensity = _material.GetColor(EmissionColor).maxColorComponent /
                               emissionColor.maxColorComponent;

        SetEmissionIntensity(currentIntensity);
    }
}

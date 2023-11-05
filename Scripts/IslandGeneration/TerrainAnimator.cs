using DG.Tweening;
using UnityEngine.Events;
using UnityEngine;

public sealed class TerrainAnimator : MonoBehaviour
{
    [SerializeField] private AnimationCurve _transitionCurve;

    [SerializeField] private float _radius;

    [SerializeField] private Material _baseMaterial;

    [SerializeField] private Material _transitionMaterial;
    public Material TransitionMaterial => _transitionMaterial;

    private MeshRenderer _meshRenderer;

    public UnityEvent AnitmationStarted;
    public UnityEvent AnimationEnded;

    private void Awake()
    {
        _transitionMaterial = new Material(_transitionMaterial);
    
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetCenter(Vector3 position)
    {
        _transitionMaterial.SetVector("Center", position);
    }
    
    private void SetRadiusToTransitionMaterial(float radius)
    {
        _transitionMaterial.SetFloat("Distance", radius);
    }

    public void StartDisappearing(float duration)
    {
        _meshRenderer.sharedMaterial = _transitionMaterial;
        
        AnitmationStarted.Invoke();

        DOVirtual.Float(_radius, 0, duration, SetRadiusToTransitionMaterial).SetEase(_transitionCurve);
    }

    public void StartAppearing(float duration)
    {
        _meshRenderer.sharedMaterial = _transitionMaterial;

        DOVirtual.Float(0, _radius, duration, SetRadiusToTransitionMaterial).SetEase(_transitionCurve).OnComplete(Appear);
    }

    private void Appear()
    {
        _meshRenderer.sharedMaterial = _baseMaterial;

        AnimationEnded.Invoke();
    }    
}

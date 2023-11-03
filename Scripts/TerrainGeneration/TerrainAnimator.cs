using System.Collections;
using UnityEngine.Events;
using UnityEngine;

public sealed class TerrainAnimator : MonoBehaviour
{
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

    public void StartDisappearing(float duration)
    {
        StopAllCoroutines();

        _meshRenderer.sharedMaterial = _transitionMaterial;

        StartCoroutine(Disappear(duration));

        AnitmationStarted.Invoke();
    }

    private IEnumerator Disappear(float duration)
    {
        YieldInstruction instruction = new WaitForFixedUpdate();

        float elapsedTime = 0f;

        while (duration >= elapsedTime)
        {
            elapsedTime += Time.deltaTime;

            float evaluatedValue = elapsedTime / duration;

            float currentRadius = Mathf.Lerp(_radius, 0f, evaluatedValue);

            _transitionMaterial.SetFloat("Distance", currentRadius);
            
            yield return instruction;
        }
    }

    public void StartAppearing(float duration)
    {
        StopAllCoroutines();

        _meshRenderer.sharedMaterial = _transitionMaterial;

        StartCoroutine(Appear(duration));
    }

    private IEnumerator Appear(float duration)
    {
        YieldInstruction instruction = new WaitForFixedUpdate();

        float elapsedTime = 0f;

        while (duration >= elapsedTime)
        {
            elapsedTime += Time.deltaTime;

            float evaluatedValue = elapsedTime / duration;

            float currentRadius = Mathf.Lerp(0f, _radius, evaluatedValue);

            _transitionMaterial.SetFloat("Distance", currentRadius);
            
            yield return instruction;
        }
        
        _meshRenderer.sharedMaterial = _baseMaterial;
        
        AnimationEnded.Invoke();
    }    
}

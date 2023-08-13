using System.Collections;
using UnityEngine.Events;
using UnityEngine;

public sealed class TerrainAnimator : MonoBehaviour
{
    [SerializeField] private float _radius;

    [SerializeField] private Material _baseMaterial;

    [SerializeField] private Material _transitionMaterial;

    private Material _currentTransitionMaterial;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        _currentTransitionMaterial = new Material(_transitionMaterial);
    }

    public void SetCenter(Vector3 position)
    {
        _currentTransitionMaterial.SetVector("Center", position);
    }

    public void StartDisappearing(float duration)
    {
        StopAllCoroutines();

        StartCoroutine(Disappear(duration));
        
        _meshRenderer.material = _currentTransitionMaterial;
    }

    private IEnumerator Disappear(float duration)
    {
        YieldInstruction instruction = new WaitForFixedUpdate();

        float elapsedTime = 0f;

        while (duration >= elapsedTime)
        {
            yield return instruction;

            elapsedTime += Time.deltaTime;

            float evaluatedValue = elapsedTime / duration;

            float currentRadius = Mathf.Lerp(_radius, 0f, evaluatedValue);

            _currentTransitionMaterial.SetFloat("Distance", currentRadius);
        }

        _meshRenderer.material = _baseMaterial;
    }

    public void StartAppearing(float duration)
    {
        StopAllCoroutines();

        StartCoroutine(Appear(duration));
        
        _meshRenderer.material = _currentTransitionMaterial;
    }

    private IEnumerator Appear(float duration)
    {
        YieldInstruction instruction = new WaitForFixedUpdate();

        float elapsedTime = 0f;

        while (duration >= elapsedTime)
        {
            yield return instruction;

            elapsedTime += Time.deltaTime;

            float evaluatedValue = elapsedTime / duration;

            float currentRadius = Mathf.Lerp(0f, _radius, evaluatedValue);

            _currentTransitionMaterial.SetFloat("Distance", currentRadius);
        }

        _meshRenderer.material = _baseMaterial;
    }    
}

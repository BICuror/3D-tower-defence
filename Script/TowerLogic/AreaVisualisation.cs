using System.Collections;
using UnityEngine;

public sealed class AreaVisualisation : MonoBehaviour
{
    [Header("VisualisationSettings")]
    [SerializeField] private int _visualisationFrameDuration;

    [Range(0f, 2f)] [SerializeField] private float _visualisationSpeed;

    [SerializeField] private GameObject _reachAreaVisualisation;

    public void ActivateVisualisation(GameObject draggable)
    {
        if (draggable.TryGetComponent<AreaManager>(out AreaManager manager))
        {
            StopAllCoroutines();

            _reachAreaVisualisation.SetActive(true);

            StartCoroutine(VisualisationAppear(manager.GetScale()));
        }
    }
    
    private IEnumerator VisualisationAppear(Vector3 finalScale)
    {
        YieldInstruction instruction = new WaitForFixedUpdate();

        for (float i = 0; i < _visualisationFrameDuration; i++)
        {
            yield return instruction;

            float scale = Mathf.Lerp(_reachAreaVisualisation.transform.localScale.x, finalScale.x, i / (float)(_visualisationFrameDuration)) * _visualisationSpeed;

            _reachAreaVisualisation.transform.localScale = new Vector3(scale, finalScale.y, scale);
        }  
    }

    public void DisactivateVisualisation(GameObject draggable)
    {
        if (draggable.TryGetComponent<AreaManager>(out AreaManager manager))
        {
            StopAllCoroutines();

            StartCoroutine(VisualisationDisappear(manager.GetScale()));
        }
    }

    private IEnumerator VisualisationDisappear(Vector3 initialScale)
    {
        YieldInstruction instruction = new WaitForFixedUpdate();

        for (float i = 0; i < _visualisationFrameDuration; i++)
        {
            yield return instruction;

            float scale = Mathf.Lerp(_reachAreaVisualisation.transform.localScale.x, 0, i / (float)(_visualisationFrameDuration) * _visualisationSpeed);

            _reachAreaVisualisation.transform.localScale = new Vector3(scale, initialScale.y, scale);
        }
        
        _reachAreaVisualisation.SetActive(false);
    }
}

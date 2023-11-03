using System.Collections;
using UnityEngine;

public sealed class AreaVisualisation : MonoBehaviour
{
    [Header("VisualisationSettings")]
    [SerializeField] private int _visualisationFrameDuration;

    [SerializeField] private AnimationCurve _appearSpeedCurve;

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

        for (float i = 0; i <= _visualisationFrameDuration; i++)
        {
            yield return instruction;

            float scale = Mathf.Lerp(0f, finalScale.x, _appearSpeedCurve.Evaluate(i / (float)(_visualisationFrameDuration)));

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

        for (float i = 0; i <= _visualisationFrameDuration; i++)
        {
            yield return instruction;

            float scale = Mathf.Lerp(initialScale.x, 0, _appearSpeedCurve.Evaluate(i / (float)(_visualisationFrameDuration)));

            _reachAreaVisualisation.transform.localScale = new Vector3(scale, initialScale.y, scale);
        }
        
        _reachAreaVisualisation.SetActive(false);
    }
}

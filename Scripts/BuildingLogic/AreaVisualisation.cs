using DG.Tweening;
using UnityEngine;

public sealed class AreaVisualisation : MonoBehaviour
{
    [Header("Curves")]
    [SerializeField] private AnimationCurve _visualisationAppearCurve;
    [SerializeField] private AnimationCurve _visualisationDisappearCurve;

    [Header("VisualisationSettings")]
    [SerializeField] private float _visualisationDuration;

    [SerializeField] private GameObject _reachAreaVisualisation;

    private Tween _currentTween;

    private void SetVisualisationScale(Vector3 scale)
    {
        _reachAreaVisualisation.transform.localScale = scale;
    }

    public void ActivateVisualisation(GameObject draggable)
    {
        if (draggable.TryGetComponent<AreaManager>(out AreaManager manager))
        {
            _reachAreaVisualisation.SetActive(true);

            if (_currentTween == null || _currentTween.IsPlaying()) _currentTween.Kill();

            _currentTween = DOVirtual.Vector3(Vector3.zero, manager.GetScale(), _visualisationDuration, SetVisualisationScale).SetEase(_visualisationAppearCurve);
        }
    }

    public void DisactivateVisualisation(GameObject draggable)
    {
        if (draggable.TryGetComponent<AreaManager>(out AreaManager manager))
        {   
            if (_currentTween == null || _currentTween.IsPlaying()) _currentTween.Kill();

            _currentTween = DOVirtual.Vector3(manager.GetScale(), Vector3.zero, _visualisationDuration, SetVisualisationScale).SetEase(_visualisationDisappearCurve).OnComplete(DisableVisualisationObject);
        }
    }

    private void DisableVisualisationObject() => _reachAreaVisualisation.SetActive(false);
}

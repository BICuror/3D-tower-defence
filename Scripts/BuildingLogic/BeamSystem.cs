using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class BeamSystem : MonoBehaviour
{
    enum BeamType
    {
        Static,
        Dynamic
    }

    [SerializeField] private BeamType _beamType;

    [SerializeField] private int _beamTransitionFrameDuration;

    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] private Transform _beamSource;

    public UnityEvent BeamConnected;

    private Vector3 _currentBeamPosition;

    private Transform _currentBeamTarget;

    private YieldInstruction _rechargeInstruction;

    private void Awake()
    {
        _rechargeInstruction = new WaitForFixedUpdate();
    
        _currentBeamPosition = _beamSource.position;
    }

    public void ReturnBeam() => StartBeamTranstionToPosition(_beamSource);

    public void SetBeamPositionToSource() => _currentBeamPosition = _beamSource.position;

    public void DisableBeam()
    {
        StopAllCoroutines();

        _lineRenderer.positionCount = 0;

        _currentBeamTarget = null;

        _currentBeamPosition = _beamSource.position;
    }

    public void StartBeamTranstionToPosition(Transform target) 
    {
        if (_currentBeamTarget != null && _currentBeamTarget == target) return;
        _currentBeamTarget = target;

        StopAllCoroutines();

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPositions(new Vector3[2]{_beamSource.position, _currentBeamPosition});
    
        StartCoroutine(TransformBeamToPosition());
    }

    private IEnumerator TransformBeamToPosition()
    {
        Vector3 startPosition = _currentBeamPosition;

        for (float i = 0; i <= _beamTransitionFrameDuration; i++)
        {
            yield return _rechargeInstruction;

            _currentBeamPosition = Vector3.Lerp(startPosition, _currentBeamTarget.position, i / (float)_beamTransitionFrameDuration);
        
            _lineRenderer.SetPosition(1, _currentBeamPosition);
        }

        if (_beamSource == _currentBeamTarget) DisableBeam();
        else if (_beamType == BeamType.Dynamic) StartCoroutine(KeepUpBeamToTarget());

        BeamConnected.Invoke();
    }

    private IEnumerator KeepUpBeamToTarget()
    {
        while (true)
        {
            _currentBeamPosition = _currentBeamTarget.position;

            _lineRenderer.SetPosition(1, _currentBeamTarget.position);

            yield return _rechargeInstruction;
        }
    }
}

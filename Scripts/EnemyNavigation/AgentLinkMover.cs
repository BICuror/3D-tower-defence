using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]

public sealed class AgentLinkMover : MonoBehaviour
{
    [SerializeField] private float _jumpHeightInBlocks; 
    [SerializeField] private AnimationCurve _animationCurve;
    
    [SerializeField] private float _linkTravelDuration = 1f;

    private NavMeshAgent _agent;
    private bool _isOnNavMeshLink;
    private YieldInstruction _yieldInstruction = new WaitForFixedUpdate();

    private void Awake() => _agent = GetComponent<NavMeshAgent>();
    private void OnEnable() 
    {
        if (_isOnNavMeshLink)
        {    
            _isOnNavMeshLink = false;
        }
    }

    private void Update()
    {
        if (_agent.isOnOffMeshLink)
        {
            if (_isOnNavMeshLink == false)
            {
                _isOnNavMeshLink = true;
                StartCoroutine(TravelByParabola());
            }
        }
    }

    private IEnumerator TravelByParabola()
    {
        OffMeshLinkData currentData = _agent.currentOffMeshLinkData;

        float duration = _linkTravelDuration * (1f / _agent.speed);

        Vector3 startPos = _agent.transform.position;
        Vector3 endPos = currentData.endPos + Vector3.up * _agent.baseOffset;

        float passedTime = 0f;

        while (passedTime < duration)
        {
            float evaluatedTime = passedTime / duration;

            float yOffset = _jumpHeightInBlocks * _animationCurve.Evaluate(evaluatedTime);
            
            _agent.transform.position = Vector3.Lerp(startPos, endPos, evaluatedTime) + yOffset * Vector3.up;
            
            passedTime += Time.deltaTime;
            
            yield return _yieldInstruction;
        }
        
        _isOnNavMeshLink = false;

        transform.position = endPos;

        _agent.CompleteOffMeshLink();
    }
}

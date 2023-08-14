using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum OffMeshLinkMoveMethod
{
    Teleport,
    NormalSpeed,
    Parabola,
    Curve
}

[RequireComponent(typeof(NavMeshAgent))]

public class AgentLinkMover : MonoBehaviour
{
    [SerializeField] private OffMeshLinkMoveMethod m_Method = OffMeshLinkMoveMethod.Parabola;
    [SerializeField] private AnimationCurve m_Curve = new AnimationCurve();

    private NavMeshAgent agent;

    private float _linkDuration = 1f;

    private YieldInstruction _yieldInstruction = new WaitForFixedUpdate();

    private void Awake() => agent = GetComponent<NavMeshAgent>();
    private void Update()
    {
        if (agent.isOnOffMeshLink)
        {
            StartCoroutine(Parabola(agent, 2.0f, 0.5f));
            agent.CompleteOffMeshLink();
        }
    }

    private IEnumerator NormalSpeed(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        while (agent.transform.position != endPos)
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
            yield return _yieldInstruction;
        }
    }

    private IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < _linkDuration)
        {
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return _yieldInstruction;
        }
    }

    private IEnumerator Curve(NavMeshAgent agent, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < _linkDuration)
        {
            float yOffset = m_Curve.Evaluate(normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / (duration * 4);
            yield return _yieldInstruction;
        }
    }
}

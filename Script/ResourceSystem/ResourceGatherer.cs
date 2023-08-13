using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public sealed class ResourceGatherer : MonoBehaviour
{
    [SerializeField] private float _damageAmount;

    public UnityEvent ReachedDestination;

    public UnityEvent SourceDestroyed;

    private ResourceSource _currentSource;

    private NavMeshAgent _agent;

    private void Awake() => _agent = GetComponent<NavMeshAgent>();

    public void FindObjectToGather()
    {
        List<ResourceSource> resourceSourceList = ResourceSourcesList.GetList();

        for (int i = resourceSourceList.Count - 1; i >= 0; i--)
        {
            if (!HasSameHeight(resourceSourceList[i].transform.position.y) || !IsReachable(resourceSourceList[i].transform.position) && Vector3.Distance(transform.position, resourceSourceList[i].transform.position) > 2f)
            {
                resourceSourceList.RemoveAt(i);
            }
        }

        float minimalDistance = float.MaxValue;
        int bestIndex = -1;

        for (int i = 0; i < resourceSourceList.Count; i++)
        {
            float currentDistance = Vector3.Distance(resourceSourceList[i].transform.position, transform.position);

            if (currentDistance < minimalDistance)
            {
                minimalDistance = currentDistance;
                bestIndex = i;
            }
        }

        if (bestIndex != -1)
        {
            _currentSource = resourceSourceList[bestIndex];

            _agent.SetDestination(_currentSource.transform.position);

            StartCoroutine(CheckIfReachedDestination());
        }
    }   
    
    private bool IsReachable(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(position, path);
        
        return (path.status != NavMeshPathStatus.PathPartial);
    }

    private bool HasSameHeight(float value)
    {
        return (int)(transform.position.y) == (int)(value);
    }

    private IEnumerator CheckIfReachedDestination()
    {
        YieldInstruction instruction = new WaitForSeconds(0.25f);

        while (Vector3.Distance(transform.position, _currentSource.transform.position) > 1f)
        {
            yield return instruction;
        }  

        ReachedDestination.Invoke();
    }

    private void GatherResources()
    {
        _currentSource.DecreaseDurability(_damageAmount);

        if (_currentSource.IsEmpty())
        {
            FindObjectToGather();

            SourceDestroyed.Invoke();
        }
    }
}

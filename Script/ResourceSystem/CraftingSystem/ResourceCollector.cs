using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class ResourceCollector : MonoBehaviour
{
    public UnityEvent CollectedResources;

    [SerializeField] private float _launchMaxHeight;  

    [SerializeField] private float _collectionDuration;

    private YieldInstruction _yieldInstruction = new WaitForFixedUpdate();

    public void CollectResources(IReadOnlyList<Resource> resources)
    {
        StartCoroutine(DragItems(resources));
    }

    private IEnumerator DragItems(IReadOnlyList<Resource> resources)
    {
        Vector3[] startPositions = new Vector3[resources.Count];

        for (int i = 0; i < startPositions.Length; i++)
        {
            startPositions[i] = resources[i].transform.position;
        } 

        float elapsedTime = 0f;

        while (elapsedTime < _collectionDuration)
        {
            yield return _yieldInstruction;

            elapsedTime += Time.deltaTime;

            float currentProgress = elapsedTime / _collectionDuration;

            for (int i = 0; i < startPositions.Length; i++)
            {
                Vector3 evaluetedPosition = Vector3.Lerp(startPositions[i], transform.position, currentProgress);

                evaluetedPosition.y = evaluetedPosition.y + (Mathf.Sin(currentProgress * 180f * Mathf.Deg2Rad)) * _launchMaxHeight; 

                resources[i].transform.position = evaluetedPosition;
            }
        }

        for (int i = 0; i < resources.Count; i++)
        {
            Destroy(resources[i].gameObject);
        }

        CollectedResources.Invoke();
    }
}

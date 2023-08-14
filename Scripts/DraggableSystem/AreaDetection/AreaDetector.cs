using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AreaDetector <T> : MonoBehaviour
{
    protected List<T> _list = new List<T>();

    public UnityEvent<T> AddedEntity;
    public UnityEvent<T> RemovedEntity;

    public UnityEvent<GameObject> AddedGameObject;
    public UnityEvent<GameObject> RemovedGameObject;
    
    public bool IsEmpty() => _list.Count == 0;

    public T GetRandomEntity() => _list[Random.Range(0, _list.Count)];

    public T GetFirstEntity() => _list[0];
    
    public IReadOnlyList<T> GetList() => _list;

    public void ClearList() => _list = new List<T>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out T component))
        {
            _list.Add(component);

            AddedEntity.Invoke(component);

            AddedGameObject.Invoke(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out T component))
        {
            _list.Remove(component);

            RemovedEntity.Invoke(component);

            RemovedGameObject.Invoke(other.gameObject);
        }
    }
}

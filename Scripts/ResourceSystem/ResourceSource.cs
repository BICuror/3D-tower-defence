using UnityEngine;
using UnityEngine.Events;

public sealed class ResourceSource : MonoBehaviour
{
    public UnityEvent<ResourceSource> SourceDestroyed;

    [SerializeField] private DraggableObject _resourcePrefab;

    [SerializeField] private int _amountOfRecourses;
    private int _createdResources;

    [SerializeField] private EntityHealth _entityHealth;

    private void Awake()
    {
        _entityHealth.DeathEvent.AddListener(DestroySource);
    
        _entityHealth.Damaged.AddListener(CheckToCreateResource);
    }

    public void DestroySource(GameObject obj)
    {
        CreateAllLeftResourses();

        SourceDestroyed.Invoke(this);
    }

    private void CreateAllLeftResourses()
    {
        for (int i = 0; i < _amountOfRecourses - _createdResources; i++)
        {
            DraggableCreator.Instance.CreateDraggableOnRandomPosition(_resourcePrefab, transform.position);
        }
    }

    private void CheckToCreateResource()
    {
        float health = _entityHealth.GetHealthPrcentage();

        float missingHealth = 1f - health;

        float step = 1f / (float)_amountOfRecourses;

        int resourcesToCreate = (int)((missingHealth - (_createdResources * step)) / step);

        _createdResources += resourcesToCreate;

        for (int i = 0; i < resourcesToCreate; i++)
        {
            DraggableCreator.Instance.CreateDraggableOnRandomPosition(_resourcePrefab, transform.position);
        }
    }
}


using UnityEngine;
using UnityEngine.Events;

public sealed class ResourceSource : MonoBehaviour
{
    public UnityEvent<ResourceSource> Died;

    [SerializeField] private DraggableObject _resourcePrefab;

    [SerializeField] private int _amountOfRecourses;
    private int _createdResources;

    [SerializeField] private EntityHealth _entityHealth;

    private void Awake()
    {
        _entityHealth.DestroyEvent.AddListener(DestroySource);
    }

    public void DecreaseDurability(float amount)
    {
        _entityHealth.GetHurt(amount);

        CheckToCreateResource(_entityHealth.GetHealthPrcentage());
    }

    public void DestroySource(GameObject obj)
    {
        Died.Invoke(this);
    }

    public bool IsEmpty() => _createdResources == _amountOfRecourses;

    public void CheckToCreateResource(float health)
    {
        if (health < 0 ) health = 0;

        float missingHealth = 1f - health;

        float step = 1f / (float)_amountOfRecourses;

        int resourcesToCreate = Mathf.RoundToInt((missingHealth - (_createdResources * step)) / step);

        _createdResources += resourcesToCreate;

        for (int i = 0; i < resourcesToCreate; i++)
        {
            DraggableCreator.Instance.CreateDraggable(_resourcePrefab, transform.position);
        }
    }
}


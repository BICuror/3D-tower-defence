using UnityEngine;
using System.Collections.Generic;

public sealed class Crafter : MonoBehaviour
{
    [SerializeField] private Craft[] _crafts;
    private Craft _currentCraft;

    [SerializeField] private ResourceAreaDetector _resourceAreaDetector;

    [SerializeField] private PresentationObject _presentationObject;

    [SerializeField] private Transform _sourcePosition;

    [SerializeField] private ResourceCollector _resourceCollector; 

    private bool _resourcesCollected;
    private bool _presentationObjectPlaced;

    private void Start()
    {
        _presentationObject.PickedUp.AddListener(CollectResources);
        _presentationObject.Placed.AddListener(OnPresentationObjectPlaced);
        _resourceCollector.CollectedResources.AddListener(OnResourcesCollected);
    }

    private void OnResourcesCollected()
    {
        _resourcesCollected = true;

        CheckToCreateCraftResult();
    } 

    private void OnPresentationObjectPlaced()
    {
        _presentationObjectPlaced = true;

        CheckToCreateCraftResult();
    }

    private void CheckToCreateCraftResult()
    {
        if (_resourcesCollected && _presentationObjectPlaced)
        {
            CreateCraftResult();

            _resourcesCollected = false;

            _presentationObjectPlaced = false;
        }
    }
    
    private void CreateCraftResult()
    {
        DraggableCreator.Instance.CreateDraggableOnPosition(_currentCraft.ResultPrefab, transform.position, _presentationObject.transform.position);

        _currentCraft = null;
        
        _presentationObject.transform.position = _sourcePosition.position;
        
        _presentationObject.gameObject.SetActive(false);
    }

    private void CollectResources()
    {
        List<Resource> resourcesInArea = new List<Resource>(_resourceAreaDetector.GetPlacedComponentsList());

        _resourceCollector.CollectResources(resourcesInArea);

        _resourceAreaDetector.ClearList();

        _resourceAreaDetector.ClearPlacedList();
    }

    public void TryToCraft()
    {
        Dictionary<ResourceData, int> resources = GetResourcesInArea();

        _currentCraft = TryFindSutableCraft(resources);

        if (_currentCraft != null)
        {
            _presentationObject.gameObject.SetActive(true);

            _presentationObject.UpdateVisuals(_currentCraft.ResultPrefab.gameObject.GetComponent<MeshFilter>().sharedMesh);
        }
        else
        {
            _presentationObject.gameObject.SetActive(false);
        }
    }

    private Dictionary<ResourceData, int> GetResourcesInArea()
    {
        IReadOnlyList<Resource> resourcesInArea = _resourceAreaDetector.GetPlacedComponentsList();
    
        Dictionary<ResourceData, int> resources = new Dictionary<ResourceData, int>();

        for (int i = 0; i < resourcesInArea.Count; i++)
        {
            if (resources.ContainsKey(resourcesInArea[i].GetResourceData()) == false)
            {
                resources.Add(resourcesInArea[i].GetResourceData(), 1);
            }
            else 
            {   
                resources[resourcesInArea[i].GetResourceData()] += 1;
            }
        }

        return resources;
    }

    private Craft TryFindSutableCraft(Dictionary<ResourceData, int> resources)
    {
        for (int craftIndex = 0; craftIndex < _crafts.Length; craftIndex++)
        {
            if (IsSutableCraft(_crafts[craftIndex], resources))
            {
                return _crafts[craftIndex];
            }
        }

        return null;
    }

    private bool IsSutableCraft(Craft craft, Dictionary<ResourceData, int> resources)
    {
        IReadOnlyList<CraftRequirement> craftRequirements = craft.GetCraftRequirements();

        if (craftRequirements.Count != resources.Count) return false;

        for (int x = 0; x < craftRequirements.Count; x++)
        {
            bool hasNeededResource = false;

            for (int y = 0; y < resources.Count; y++)
            {
                if (resources.ContainsKey(craftRequirements[x].Resource))
                {
                    if (resources[craftRequirements[x].Resource] == craftRequirements[x].ResourceAmount)
                    {
                        hasNeededResource = true;

                        break;
                    }
                }
            }

            if (hasNeededResource == false) return false;
        }

        return true;
    }
}

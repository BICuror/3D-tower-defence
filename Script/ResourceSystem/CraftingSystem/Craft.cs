using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Craft", menuName = "ScriptableObject/Craft")]

public sealed class Craft : ScriptableObject 
{
    [SerializeField] private List<CraftRequirement> _craftRequirements;

    [SerializeField] private DraggableObject _resultPrefab;
    public DraggableObject ResultPrefab => _resultPrefab;

    public IReadOnlyList<CraftRequirement> GetCraftRequirements() => _craftRequirements; 
}

[System.Serializable] public struct CraftRequirement
{
    [SerializeField] private ResourceData _resource;
    public ResourceData Resource => _resource;

    [SerializeField] private int _resourceAmount;
    public int ResourceAmount => _resourceAmount;
}

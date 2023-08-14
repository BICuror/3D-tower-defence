using UnityEngine;

[CreateAssetMenu(fileName = "ResourceData", menuName = "ScriptableObjects/ResourceData")]

public sealed class ResourceData : ScriptableObject 
{
    [SerializeField] private string _resourceName;
    public string Name {get => _resourceName;}
}

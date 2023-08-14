using UnityEngine;

public sealed class Resource : MonoBehaviour
{
    [SerializeField] private ResourceData _resourceData;
    public ResourceData GetResourceData() => _resourceData;
} 

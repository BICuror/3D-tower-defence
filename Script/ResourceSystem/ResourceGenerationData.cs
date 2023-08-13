using UnityEngine;

[CreateAssetMenu(fileName = "ResourceGenerationData", menuName = "ScriptableObject/ResourceGenerationData")]

public sealed class ResourceGenerationData : ScriptableObject
{
    [SerializeField] private GameObject _sourcePrefab;
    public GameObject SourcePrefab {get => _sourcePrefab;} 

    [SerializeField] private int _sourcesAmount;
    public int SourcesAmount {get => _sourcesAmount;} 

    [SerializeField] private int _minimalDistanceFromOtherSources;
    public int MinimalDistanceFromOtherSources {get => _minimalDistanceFromOtherSources;} 

    [SerializeField] private SpawnMethod _spawnMethod;
    public SpawnMethod GetSpawnMethod() => _spawnMethod;
}

public enum SpawnMethod
{
    Default, 
    CloserToCenter,
    CloserToBorders
}
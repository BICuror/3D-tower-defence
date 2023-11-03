using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class EnemyBiomeGenerator : MonoBehaviour
{
    [Inject] private IslandData _islandData;
    [Inject] private RoadNodeGenerator _roadNodeGenerator;
    [Inject] private IslandGenerator _islandGenerator;
    [Inject] private RoadMapGenerator _roadMapGenerator;
    [Inject] private SpawnerRoadNodeGenerator _spawnerRoadNodeGenerator;
    [Inject] private EnemyBiomeContainer _enemyBiomeContainer;

    [Inject] private TextureManager _textureManager;
    
    [SerializeField] private EnemyBiome _enemyBiomePrefab;

    [SerializeField] private DecorationContainer _islandDecorationContainer;

    private EnemyBiomeMeshGenerator _corruptionBiomeMeshGenerator;

    private BlockGrid _terrainBlockGrid;

    public void Setup(BlockGrid islandBlockGrid)
    {
        _terrainBlockGrid = islandBlockGrid;
    }

    private void Awake() 
    {
        _corruptionBiomeMeshGenerator = new EnemyBiomeMeshGenerator();

        _corruptionBiomeMeshGenerator.SetupGenerator(_islandGenerator.IslandGrid, _textureManager);
    }

    public void TryGenerateNewBiome()
    {
        if (_islandData.MaxAmountOfEnemyBiomes > _enemyBiomeContainer.EnemyBiomeAmount) GenerateNewBiome();
    }

    private void GenerateNewBiome()
    {
        IReadOnlyList<Vector2Int> enemyBiomesIndexes = _enemyBiomeContainer.GetEnemyBiomesNodeIndexes();

        Vector2Int spawnerNodeIndex = _spawnerRoadNodeGenerator.GetRandomEnemySpawnerNodeIndex(enemyBiomesIndexes);

        Vector2Int spawnerPosition = _roadNodeGenerator.GetNodePosition(spawnerNodeIndex);

        EnemyBiome biome = Instantiate(_enemyBiomePrefab, new Vector3(spawnerPosition.x, 0f, spawnerPosition.y), Quaternion.identity);

        biome.SetCenterPosition(spawnerPosition);    

        biome.Setup(_textureManager, _terrainBlockGrid, _islandDecorationContainer, spawnerNodeIndex, _islandData, _roadMapGenerator, _corruptionBiomeMeshGenerator);      

        _enemyBiomeContainer.AddBiome(biome);
    }
}

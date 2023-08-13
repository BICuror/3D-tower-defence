using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBiomeGenerator : MonoBehaviour
{
    [SerializeField] private NodeGenerator _nodeGenerator;

    [SerializeField] private TextureManager _textureManager;
    
    [SerializeField] private GameObject _enemyBiomePrefab;

    [SerializeField] private DecorationContainer _islandDecorationContainer;

    [SerializeField] private CorruptionBiomeMeshGenerator _corruptionBiomeMeshGenerator;

    private BlockGrid _terrainBlockGrid;

    private List<EnemyBiome> _enemyBiomes = new List<EnemyBiome>();

    public void Setup(BlockGrid islandBlockGrid)
    {
        _terrainBlockGrid = islandBlockGrid;

        _nodeGenerator.SetupNodes();

        for (int i = 0; i < IslandDataContainer.GetData().BegginingAmountOfEnemyBiomes - 1; i++)
        {
            TryGenerateNewBiome();
        }
    }

    public void DisableBiomesTerrain(float duration)
    {
        for (int i = 0; i < _enemyBiomes.Count; i++)
        {
            _enemyBiomes[i].DisableTerrain(duration);    
        }
    }

    public void EnableBiomesTerrain(float duration)
    {
        for (int i = 0; i < _enemyBiomes.Count; i++)
        {
            _enemyBiomes[i].EnableTerrain(duration);    
        }
    }

    public void IncreaseBiomesStages()
    {
        for (int i = 0; i < _enemyBiomes.Count; i++)
        {
            _enemyBiomes[i].IncreaseCurrentStage();

            if (_enemyBiomes[i].GetStage() >= IslandDataContainer.GetData().EnemyBiomeStages.Length)
            {
                Destroy(_enemyBiomes[i].gameObject);

                _enemyBiomes.RemoveAt(i);
            }
        }
    }

    public void RegenerateBiomes()
    {
        for (int i = 0; i < _enemyBiomes.Count; i++)
        {
            _enemyBiomes[i].RegenerateBiome();
        }
    }

    public void TryGenerateNewBiome()
    {
        if (IslandDataContainer.GetData().MaxAmountOfEnemyBiomes > _enemyBiomes.Count) GenerateNewBiome();
    }

    private void GenerateNewBiome()
    {
        Vector2Int enemySpawnerNode = _nodeGenerator.GetEnemySpawnerNodes(GetEnemyBiomesPositions());

        GameObject biome = Instantiate(_enemyBiomePrefab, new Vector3(enemySpawnerNode.x, 0f, enemySpawnerNode.y), Quaternion.identity);

        biome.GetComponent<EnemyBiome>().SetCenterPosition(enemySpawnerNode);    

        biome.GetComponent<EnemyBiome>().Setup(_terrainBlockGrid, _textureManager, _islandDecorationContainer, _corruptionBiomeMeshGenerator);      

        _enemyBiomes.Add(biome.GetComponent<EnemyBiome>());
    }

    public List<Vector2Int> GetEnemyBiomesPositions()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        for (int i = 0; i < _enemyBiomes.Count; i++)
        {
            result.Add(_enemyBiomes[i].GetCenterPosition());
        }
        
        return result;
    }
}

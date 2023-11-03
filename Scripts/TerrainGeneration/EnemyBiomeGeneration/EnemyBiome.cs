using UnityEngine.Events;
using UnityEngine;
using Zenject;

public sealed class EnemyBiome : MonoBehaviour
{
    private IslandData _islandData;
    private RoadMapGenerator _roadMapGenerator;
    private EnemyBiomeMeshGenerator _terrainMeshGenerator;

    [SerializeField] private TerrainSetter _terrainSetter;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private EnemyBiomeDecorationGenerator _enemyBiomeDecorationGenerator;
    [SerializeField] private TerrainAnimator _terrainAnimator;

    [SerializeField] private EnemyBiomeDecorationMaterialChanger _enemyBiomeDecorationManager;

    private Vector2Int _centerPosition;

    private BlockGrid _islandBlockGrid;
    private BlockGrid _currentBlockGrid;

    private TextureManager _textureManager;

    private Vector2Int _spawnerNodeIndex;
    public Vector2Int SpawnerNodeIndex => _spawnerNodeIndex;

    private DecorationContainer _islandDecorationContainer;

    private int _currentStage;

    public void Setup(TextureManager textureManager, BlockGrid islandBlockGrid, DecorationContainer islandDecorationContainer, Vector2Int spawnerNode, IslandData islandData, RoadMapGenerator roadMapGenerator, EnemyBiomeMeshGenerator enemyBiomeGenerator)
    {
        _islandBlockGrid = islandBlockGrid;
        _islandDecorationContainer = islandDecorationContainer;
        _spawnerNodeIndex = spawnerNode;
        _islandData = islandData;
        _roadMapGenerator = roadMapGenerator;
        _terrainMeshGenerator = enemyBiomeGenerator;
        _textureManager = textureManager;
    }

    public void SetCenterPosition(Vector2Int position)
    {
        _centerPosition = position;
    
        _terrainAnimator.SetCenter(new Vector3(position.x, transform.position.y, position.y));
    }
    
    public Vector2Int GetCenterPosition() => _centerPosition;

    public void IncreaseCurrentStage() => _currentStage++;

    public int GetStage() => _currentStage;

    private void AdjustPosition()
    {
        int radius = _islandData.EnemyBiomeStages[_currentStage].EnemyBiomeRadius;

        transform.position = new Vector3(_centerPosition.x - radius, 0f, _centerPosition.y - radius);

        float height = _islandBlockGrid.GetMaxHeight(_centerPosition.x, _centerPosition.y);

        if (height == 0) height += 1f;

        _enemySpawner.transform.localPosition = new Vector3(radius, height + 1f, radius);
    }

    public void DisableTerrain(float duration)
    {
        _terrainAnimator.StartDisappearing(duration);
    }

    public void EnableTerrain(float duration)
    {
        _terrainAnimator.StartAppearing(duration);
    }

    public void RegenerateBiome()
    {
        AdjustPosition();

        bool[,] enemyBiomeMap = GenerateEnemyMap();

        BlockGrid blockGrid = ConvertEnemyMapToGrid(enemyBiomeMap);

        GenerateMesh(blockGrid);

        _currentBlockGrid = blockGrid;
    }

    public void GenerateDecorations()
    {
        int radius = _islandData.EnemyBiomeStages[_currentStage].EnemyBiomeRadius;

        Vector2Int currentPos = new Vector2Int((int)(transform.position.x), (int)(transform.position.z));

        _enemyBiomeDecorationGenerator.SetDecorationModule(_islandData.EnemyBiomeStages[_currentStage].DecorationsModule);

        _enemyBiomeDecorationGenerator.GenerateDecorations(_currentBlockGrid, currentPos);
        
        _enemyBiomeDecorationManager.ApplyTransitionMaterial();
    }

    private void GenerateMesh(BlockGrid blockGrid)
    {
        _terrainMeshGenerator.SetupGenerator(blockGrid, _textureManager);

        Mesh mesh = _terrainMeshGenerator.GetMesh();

        _terrainSetter.SetMesh(mesh);
    }

    private bool[,] GenerateEnemyMap()
    {
        int radius = _islandData.EnemyBiomeStages[_currentStage].EnemyBiomeRadius;

        bool[,] enemyBiomeMap = new bool[radius * 2 + 1, radius * 2 + 1];

        for (int x = 0; x < radius * 2 + 1; x++)
        {
            for (int y = 0; y < radius * 2 + 1; y++)
            {
                int distance = Mathf.Abs(radius + 1 - x) + Mathf.Abs(radius + 1 - y);

                if (Random.Range(0f, 1f) > _islandData.EnemyBiomeStages[_currentStage].EnemyBiomeEdgeReductionCurve.Evaluate(Mathf.Lerp(0, 1, distance / radius)))
                {
                    enemyBiomeMap[x, y] = true;

                    _islandDecorationContainer.SetActiveDecorationsIfInBound(_centerPosition.x - radius + x, _centerPosition.y - radius + y, false);
                }
            }
        }

        return enemyBiomeMap;
    }

    private BlockGrid ConvertEnemyMapToGrid(bool[,] enemyBiomeMap)
    {
        int radius = _islandData.EnemyBiomeStages[_currentStage].EnemyBiomeRadius;

        BlockGrid enemyBiomeGrid = new BlockGrid(radius * 2 + 1, _islandData.IslandMaxHeight);

        Vector2Int currentPos = new Vector2Int((int)(transform.position.x), (int)(transform.position.z));

        bool[,] roadMap = _roadMapGenerator.RoadMap; 

        for (int x = 0; x < radius * 2 + 1; x++)
        {        
            for (int z = 0; z < radius * 2 + 1; z++)
            {   
                if (enemyBiomeMap[x, z])
                {   
                    int height = 0;

                    if (currentPos.x + x < _islandData.IslandSize && currentPos.y + z < _islandData.IslandSize && currentPos.x + x >= 0 && currentPos.y + z >= 0) 
                    {
                        if (roadMap[currentPos.x + x, currentPos.y + z] == true) continue;

                        height = _islandBlockGrid.GetMaxHeight(currentPos.x + x, currentPos.y + z);
                    }

                    BlockType blockType = BlockType.Corruption;

                    if (height <= 0) 
                    {
                        height = _islandData.CorruptionLessZeroHeight;

                        blockType = BlockType.CorruptionOnWater;
                    }
                    
                    for (int y = height; y >= 0; y--)
                    {
                        enemyBiomeGrid.SetBlockType(new Vector3Int(x, y, z), blockType);  
                    }    
                }
            }
        }

        return enemyBiomeGrid;
    }

    public void Destroy()
    {
        _enemySpawner.DeactivateSpawner();

        Destroy(gameObject);
    }
}

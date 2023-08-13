using UnityEngine;

public sealed class EnemyBiome : MonoBehaviour
{
    [SerializeField] private TerrainSetter _terrainSetter;
    
    private CorruptionBiomeMeshGenerator _terrainMeshGenerator;

    [SerializeField] private EnemySpawner _enemySpawner;

    [SerializeField] private EnemyBiomeDecorationGenerator _enemyBiomeDecorationGenerator;

    [SerializeField] private TerrainAnimator _terrainAnimator;

    private Vector2Int _centerPosition;

    private BlockGrid _islandBlockGrid;

    private TextureManager _textureManager;

    private DecorationContainer _islandDecorationContainer;

    private int _currentStage;

    public void Setup(BlockGrid islandBlockGrid, TextureManager textureManager, DecorationContainer islandDecorationContainer, CorruptionBiomeMeshGenerator corruptionBiomeMeshGenerator)
    {
        _islandBlockGrid = islandBlockGrid;

        _textureManager = textureManager;

        _islandDecorationContainer = islandDecorationContainer;

        _terrainMeshGenerator = corruptionBiomeMeshGenerator;
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
        int radius = IslandDataContainer.GetData().EnemyBiomeStages[_currentStage].EnemyBiomeRadius;

        transform.position = new Vector3(_centerPosition.x - radius, 0f, _centerPosition.y - radius);

        float height = _islandBlockGrid.GetMaxHeight((int)(transform.position.x) + radius, (int)(transform.position.y) + radius);

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
        IslandData islandData = IslandDataContainer.GetData();

        AdjustPosition();

        bool[,] enemyBiomeMap = GenerateEnemyMap();

        BlockGrid blockGrid = ConvertEnemyMapToGrid(enemyBiomeMap);

        GenerateMesh(blockGrid);

        _enemyBiomeDecorationGenerator.SetDecorationModule(islandData.EnemyBiomeStages[_currentStage].DecorationsModule);

        _enemyBiomeDecorationGenerator.GenerateDecorations(blockGrid, new Vector2Int((int)(transform.position.x), (int)(transform.position.z)));
    }

    private void GenerateMesh(BlockGrid blockGrid)
    {
        _terrainMeshGenerator.SetupGenerator(blockGrid, _textureManager);

        Mesh mesh = _terrainMeshGenerator.GetMesh();

        _terrainSetter.SetMesh(mesh);
    }

    private bool[,] GenerateEnemyMap()
    {
        int radius = IslandDataContainer.GetData().EnemyBiomeStages[_currentStage].EnemyBiomeRadius;

        bool[,] enemyBiomeMap = new bool[radius * 2 + 1, radius * 2 + 1];

        for (int x = 0; x < radius * 2 + 1; x++)
        {
            for (int y = 0; y < radius * 2 + 1; y++)
            {
                int distance = Mathf.Abs(radius + 1 - x) + Mathf.Abs(radius + 1 - y);

                if (Random.Range(0f, 1f) > IslandDataContainer.GetData().EnemyBiomeStages[_currentStage].EnemyBiomeEdgeReductionCurve.Evaluate(Mathf.Lerp(0, 1, distance / radius)))
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
        IslandData islandData = IslandDataContainer.GetData();

        int radius = islandData.EnemyBiomeStages[_currentStage].EnemyBiomeRadius;

        BlockGrid enemyBiomeGrid = new BlockGrid(radius * 2 + 1, islandData.IslandMaxHeight);

        Vector2Int currentPos = new Vector2Int((int)(transform.position.x), (int)(transform.position.z));

        for (int x = 0; x < radius * 2 + 1; x++)
        {        
            for (int z = 0; z < radius * 2 + 1; z++)
            {   
                if (currentPos.x + x < islandData.IslandSize && currentPos.y + z < islandData.IslandSize && currentPos.x + x >= 0 && currentPos.y + z >= 0) 
                {
                    if (_islandBlockGrid.GetMaxHeight(currentPos.x + x, currentPos.y + z) >= 0 && enemyBiomeMap[x, z])
                    {   
                        enemyBiomeGrid.SetBlockType(new Vector3Int(x, _islandBlockGrid.GetMaxHeight(currentPos.x + x, currentPos.y + z), z), BlockType.Corruption);  
                
                        for (int y = _islandBlockGrid.GetMaxHeight(currentPos.x + x, currentPos.y + z) - 1; y >= 0; y--)
                        {
                            enemyBiomeGrid.SetBlockType(new Vector3Int(x, y, z), BlockType.Corruption);  
                        }
                    }
                    else if (enemyBiomeMap[x, z])
                    {
                        enemyBiomeGrid.SetBlockType(new Vector3Int(x, 0, z), BlockType.Corruption); 
                    }
                }
                else if (enemyBiomeMap[x, z])
                {
                    enemyBiomeGrid.SetBlockType(new Vector3Int(x, 0, z), BlockType.Corruption); 
                }
            }
        }

        return enemyBiomeGrid;
    }
}

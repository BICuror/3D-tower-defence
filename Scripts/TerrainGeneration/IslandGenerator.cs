using UnityEngine;
using Zenject;

public sealed class IslandGenerator : MonoBehaviour
{
    #region Dependencies
    [Inject] private IslandData _islandData;

    [Inject] private TextureManager _textureManager;

    [Inject] private IslandDecorationGenerator _decorationGenerator;

    [Inject] private EnemyBiomeGenerator _enemyBiomeGenerator;

    [Inject] private ResourceSourceGenerator _resourceSourceGenerator;

    [Inject] private EnviromentCreator _enviromentCreator;

    [Inject] private BiomeMapGenerator _biomeMapGenerator;

    [Inject] private HeightMapGenerator _heightMapGenerator;

    [Inject] private WaveManager _waveManager; 

    [Inject] private RoadGenerator _roadGenerator;

    [Inject] private RoadNodeGenerator _nodeGenerator;
    #endregion

    private BlockGrid _blockGrid;
    public BlockGrid IslandGrid => _blockGrid;

    [SerializeField] private TerrainSetter _terrainSetter;

    private int[,] _heightMap;

    [SerializeField] private bool _generateTerrainAndDecorationsOnly;
    
    private void Start() => GenerateIsland();

    public void GenerateIsland()
    {
        GenerateNewSeeds();

        GetHeightMap();

        ConvertHeightMapToBlockGrid();

        GenerateTerrainMesh();
            
        _decorationGenerator.GenerateDecorations(_blockGrid, Vector2.zero);

        _nodeGenerator.SetupNodes();        

        CreateEnviroment();
        _enemyBiomeGenerator.Setup(_blockGrid);

        if (_generateTerrainAndDecorationsOnly == false) 
        {
            //_resourceSourceGenerator.GenerateResources();
            _waveManager.PrepeareWave();
        }
    }

    private void GenerateNewSeeds()
    {
        _heightMapGenerator.GenerateNewSeed();

        _biomeMapGenerator.GenerateNewSeed();
    }

    private void GetHeightMap()
    {
        _heightMap = _heightMapGenerator.GenerateHeightMap(_biomeMapGenerator);
    }
    
    private void ConvertHeightMapToBlockGrid()
    {
        _blockGrid = new BlockGrid(_islandData.IslandSize, _islandData.IslandMaxHeight);

        for (int x = 0; x < _islandData.IslandSize; x++)
        {        
            for (int z = 0; z < _islandData.IslandSize; z++)
            {
                if (_heightMap[x, z] > 0) _blockGrid.SetBlockType(new Vector3Int(x, _heightMap[x, z], z), BlockType.Surface);  

                for (int y = _heightMap[x, z] - 1; y >= 0; y--)
                {
                    _blockGrid.SetBlockType(new Vector3Int(x, y, z), BlockType.Rock);  
                }
            }
        }
    }

    private void GenerateTerrainMesh()
    {
        TerrainMeshGenerator terrainMeshGenerator = new TerrainMeshGenerator();

        terrainMeshGenerator.SetupGenerator(_blockGrid, _textureManager);

        Mesh mesh = terrainMeshGenerator.GetMesh();

        _terrainSetter.SetMesh(mesh);
    }

    private void CreateEnviroment()
    {
        int _centerPoint = Mathf.RoundToInt(_islandData.IslandSize / 2);

        _enviromentCreator.CreateEnviroment(new Vector3(_centerPoint, _heightMap[_centerPoint, _centerPoint], _centerPoint));
    }
}

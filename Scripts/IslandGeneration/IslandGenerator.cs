using UnityEngine;
using Zenject;

public sealed class IslandGenerator : MonoBehaviour
{
    #region Dependencies
    [Inject] private IslandData _islandData;
    [Inject] private TextureManager _textureManager;
    [Inject] private IslandDecorationGenerator _decorationGenerator;
    [Inject] private EnemyBiomeGenerator _enemyBiomeGenerator;
    [Inject] private EnviromentCreator _enviromentCreator;
    [Inject] private BiomeMapGenerator _biomeMapGenerator;
    [Inject] private HeightMapGenerator _heightMapGenerator;
    [Inject] private RoadGenerator _roadGenerator;
    [Inject] private RoadNodeGenerator _nodeGenerator;
    [Inject] private IslandTerrainMeshCreator _islandTerrainMeshCreator;
    #endregion

    private BlockGrid _blockGrid;
    public BlockGrid IslandGrid => _blockGrid;

    private int[,] _heightMap;

    public void GenerateIsland()
    {
        GenerateNewSeeds();

        GetHeightMap();

        ConvertHeightMapToBlockGrid();

        GenerateTerrainMesh();
            
        GenerateDecorations();

        CreateEnviroment();
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
        IslandHeightMapToGridConverter converter = new IslandHeightMapToGridConverter();

        _blockGrid = converter.Convert(_heightMap, _islandData);
    }

    private void GenerateTerrainMesh()
    {
        _islandTerrainMeshCreator.CreateMesh(_blockGrid);
    }

    private void GenerateDecorations()
    {
        _decorationGenerator.GenerateDecorations(_blockGrid, Vector2.zero); 
    }

    private void SetupNodes() => _nodeGenerator.SetupNodes();          

    private void CreateEnviroment()
    {
        int _centerPoint = Mathf.RoundToInt(_islandData.IslandSize / 2);

        _enviromentCreator.CreateEnviroment(new Vector3(_centerPoint, _heightMap[_centerPoint, _centerPoint], _centerPoint));
    }
}

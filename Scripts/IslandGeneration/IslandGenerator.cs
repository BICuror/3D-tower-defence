using UnityEngine;
using Zenject;

public sealed class IslandGenerator : MonoBehaviour
{
    #region Dependencies
    [Inject] private IslandData _islandData;
    [Inject] private TextureManager _textureManager;
    [Inject] private IslandDecorationGenerator _islandDecorationGenerator;
    [Inject] private EnemyBiomeGenerator _enemyBiomeGenerator;
    [Inject] private EnviromentCreator _enviromentCreator;
    [Inject] private BiomeMapGenerator _biomeMapGenerator;
    [Inject] private HeightMapGenerator _heightMapGenerator;
    [Inject] private RoadGenerator _roadGenerator;
    [Inject] private RoadNodeGenerator _nodeGenerator;
    [Inject] private IslandTerrainMeshCreator _islandTerrainMeshCreator;
    [Inject] private IslandGridHolder _islandGridHolder;
    [Inject] private IslandHeightMapHolder _islandHeightMapHolder;
    #endregion

    public void GenerateIsland()
    {
        GenerateNewSeeds();

        GetHeightMap();

        ConvertHeightMapToBlockGrid();

        GenerateTerrainMesh();

        SetupNodes();
            
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
        _islandHeightMapHolder.Map = _heightMapGenerator.GenerateHeightMap(_biomeMapGenerator);
    }
    
    private void ConvertHeightMapToBlockGrid()
    {
        IslandHeightMapToGridConverter converter = new IslandHeightMapToGridConverter();

        _islandGridHolder.Grid = converter.Convert(_islandHeightMapHolder.Map, _islandData);
    }

    private void GenerateTerrainMesh()
    {
        _islandTerrainMeshCreator.CreateMesh(_islandGridHolder.Grid);
    }

    private void GenerateDecorations()
    {
        _islandDecorationGenerator.GenerateDecorations(_islandGridHolder.Grid, Vector2.zero); 
    }

    private void SetupNodes() => _nodeGenerator.SetupNodes();          

    private void CreateEnviroment()
    {
        int _centerPoint = Mathf.RoundToInt(_islandData.IslandSize / 2);

        _enviromentCreator.CreateEnviroment(new Vector3(_centerPoint, _islandGridHolder.Grid.GetMaxHeight(_centerPoint, _centerPoint), _centerPoint));
    }
}

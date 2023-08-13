using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IslandGenerator : MonoBehaviour
{
    public bool GenerateDecorations;

    public bool GenerateEnemiesAndRoads;

    public UnityEvent IslandGenerated;

    private BlockGrid _blockGrid;

    [SerializeField] private TerrainSetter _terrainSetter;

    [SerializeField] private RoadGenerator _roadGenerator;

    [SerializeField] private TextureManager _textureManager;

    [SerializeField] private IslandDecorationGenerator _decorationGenerator;

    [SerializeField] private GameObject _townHall;

    [SerializeField] private EnemyBiomeGenerator _enemyBiomeGenerator;

    [SerializeField] private ResourceSourceGenerator _resourceSourceGenerator;

    [SerializeField] private TerrainAnimator _roadAnimator;

    private BiomeMapGenerator _biomeMapGenerator;

    private HeightMapGenerator _heightMapGenerator;

    private int[,] _heightMap;
    
    private void Start() => GenerateIsland();

    public void GenerateIsland()
    {
        SetupBiomes();

        SetupHeightMap();
        
        _textureManager.SetBiomeMap(_biomeMapGenerator);

        ConvertHeightMapToBlockGrid();

        GenerateTerrainMesh();

        if (GenerateDecorations)
        {
            _decorationGenerator.SetBiomeMap(_biomeMapGenerator);
            
            _decorationGenerator.GenerateDecorations(_blockGrid, Vector2Int.zero);
        }
        
        if (GenerateEnemiesAndRoads)
        {
            _enemyBiomeGenerator.Setup(_blockGrid); 

            _roadAnimator.SetCenter(new Vector3((IslandDataContainer.GetData().IslandSize - 1) / 2, 0f, (IslandDataContainer.GetData().IslandSize - 1) / 2));
        }

        InstantiateTownhall();

        _resourceSourceGenerator.GenerateResources();

        IslandGenerated.Invoke();
    }

    public void GenerateRoads()
    {
        _roadGenerator.GenerateRoads(_heightMap);
    }

    private void SetupBiomes()
    {
        _biomeMapGenerator = new BiomeMapGenerator();

        _biomeMapGenerator.GenerateNewBiomeSeed();
    }

    private void SetupHeightMap()
    {
        _heightMapGenerator = new HeightMapGenerator();

        _heightMapGenerator.SetNewHeightSeed();

        _heightMap = _heightMapGenerator.GenerateHeightMap(_biomeMapGenerator);
    }
    
    private void ConvertHeightMapToBlockGrid()
    {
        _blockGrid = new BlockGrid(IslandDataContainer.GetData().IslandSize, IslandDataContainer.GetData().IslandMaxHeight);

        for (int x = 0; x < IslandDataContainer.GetData().IslandSize; x++)
        {        
            for (int z = 0; z < IslandDataContainer.GetData().IslandSize; z++)
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

    private void InstantiateTownhall()
    {
        int _centerPoint = Mathf.RoundToInt(IslandDataContainer.GetData().IslandSize / 2);

        _townHall.transform.position = new Vector3(_centerPoint, _heightMap[_centerPoint, _centerPoint] + 1f, _centerPoint);

        Instantiate(IslandDataContainer.GetData().TownHallPrefab, _townHall.transform.position, Quaternion.identity, _townHall.transform);
    }
}

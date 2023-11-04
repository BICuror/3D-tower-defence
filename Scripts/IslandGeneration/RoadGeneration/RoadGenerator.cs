using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public sealed class RoadGenerator : MonoBehaviour
{
    [Inject] private IslandData _islandData;
    [Inject] private TextureManager _textureManager;
    [Inject] private HeightMapGenerator _heightMapGenerator;
    [Inject] private RoadMapGenerator _roadMapGenerator;
    [Inject] private EnemyBiomeGenerator _enemyBiomeGenerator;
    [Inject] private IslandDecorationContainer _islandDecorationContainer;
    [Inject] private NavMeshLinksGenerator _navMeshLinksGenerator;
    [Inject] private RoadNodeGenerator _nodeGenerator;

    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private TerrainSetter _roadTerrainSetter;

    public void GenerateRoads()
    {
        int[,] heightMap = _heightMapGenerator.HeightMap;

        bool[,] roadMap = _roadMapGenerator.GenerateRoads();
          
        GenerateRoadMesh(ConvertRoadBlockGrid(roadMap, heightMap));

        GenerateNavMesh(roadMap, CreateNewHeaightMap(roadMap, heightMap));
    }

    private void GenerateNavMesh(bool[,] roadMap, int[,] heightMap)
    {
        _navMeshSurface.BuildNavMesh();
        
        _navMeshLinksGenerator.DestroyAllStairs();

        _navMeshLinksGenerator.GenrateStairs(heightMap, roadMap);
    }

    private void GenerateRoadMesh(BlockGrid roadBlockGrid)
    {
        RoadMeshGenerator roadMeshGenerator = new RoadMeshGenerator();

        roadMeshGenerator.SetupGenerator(roadBlockGrid, _textureManager);
        
        Mesh roadMesh = roadMeshGenerator.GetMesh();
        
        _roadTerrainSetter.SetMesh(roadMesh); 
    }

    private BlockGrid ConvertRoadBlockGrid(bool[,] roadMap, int[,] heightMap)
    {
        BlockGrid roadGrid = new BlockGrid(_islandData.IslandSize, _islandData.IslandMaxHeight);

        for (int x = 0; x < _islandData.IslandSize; x++)
        {        
            for (int z = 0; z < _islandData.IslandSize; z++)
            {
                if (roadMap[x, z] == true) 
                {
                    if (heightMap[x, z] == 0) 
                    {
                        roadGrid.SetBlockType(new Vector3Int(x, 1, z), BlockType.RoadOnWater);  
                    }
                    else
                    {
                        roadGrid.SetBlockType(new Vector3Int(x, heightMap[x, z], z), BlockType.Road);  
                    }
                    

                    _islandDecorationContainer.SetActiveDecorations(x, z, false);
                }
            }
        }

        return roadGrid;
    }

    private int[,] CreateNewHeaightMap(bool[,] roadMap, int[,] heightMap)
    {
        int[,] roadHeightMap = new int[_islandData.IslandSize, _islandData.IslandSize];

        for (int x = 0; x < _islandData.IslandSize; x++)
        {        
            for (int z = 0; z < _islandData.IslandSize; z++)
            {
                if (roadMap[x, z])
                {
                    if (heightMap[x, z] == 0)
                    {
                        roadHeightMap[x, z] = 1;
                    }
                    else
                    {
                        roadHeightMap[x, z] = heightMap[x, z];
                    }
                }
            }
        }

        return roadHeightMap;
    }
}

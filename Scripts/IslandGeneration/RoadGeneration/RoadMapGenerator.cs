using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class RoadMapGenerator : MonoBehaviour
{
    [Inject] private IslandData _islandData;
    [Inject] private EnemyBiomeContainer _enemyBiomeContainer;
    [Inject] private RoadNodeGenerator _roadNodeGenerator;
    [Inject] private RoadMapHolder _roadMapHolder;

    private bool[,] _roadMap;

    private bool[,] _touchedNodesMap;

    public bool[,] GenerateRoads()
    {
        List<Vector2Int> spawnerNodes = _enemyBiomeContainer.GetEnemyBiomesPositions();

        Vector2Int[,] roadNodes = _roadNodeGenerator.GetAllNodes();

        _roadMap = new bool[_islandData.IslandSize, _islandData.IslandSize];

        _touchedNodesMap = new bool[_islandData.AmountOfRoadNodesBetweenCenterAndEdge * 2 + 3, _islandData.AmountOfRoadNodesBetweenCenterAndEdge * 2 + 3];

        for (int i = 0; i < spawnerNodes.Count; i++)
        {  
            if (_islandData.RoadTypeToGenerate == IslandData.RoadType.Node) CreateSpawnerRoad(spawnerNodes, roadNodes, i);
            else if (_islandData.RoadTypeToGenerate == IslandData.RoadType.Random) CreateRandomSpawnerRoad(spawnerNodes, roadNodes, i);
        }

        AddCenterRoad();

        _roadMapHolder.SetRoadMap(_roadMap);

        return _roadMap;
    }

    private void CreateRandomSpawnerRoad(List<Vector2Int> spawnerNodes, Vector2Int[,] roadNodes, int index)
    {
        Vector2Int current = FindNodeIndex(spawnerNodes[index], roadNodes, _islandData.AmountOfRoadNodesBetweenCenterAndEdge * 2 + 3);
        current = roadNodes[current.x, current.y];

        int middleIndex = (_islandData.IslandSize - 1) / 2;

        while(current.x != middleIndex || current.y != middleIndex)
        {
            float rand = Random.Range(0f, 1f);

            _roadMap[current.x, current.y] = true;

            Vector2Int direction = new Vector2Int(NormalizeNumber(middleIndex - current.x), NormalizeNumber(middleIndex - current.y));
            
            if (direction.x == 0) direction.x = NormalizeNumber(Random.Range(1, 2));
            else if (direction.y == 0) direction.y = NormalizeNumber(Random.Range(1, 2));

            if (rand < 0.35f && current.x + direction.x < _islandData.IslandSize && current.x + direction.x >= 0)  current.x += direction.x;
            else if (rand < 0.7f && current.y + direction.y < _islandData.IslandSize && current.y + direction.y >= 0) current.y += direction.y;
            else if (rand < 0.85f && current.x - direction.x < _islandData.IslandSize && current.x - direction.x >= 0) current.x -= direction.x;
            else if (current.y - direction.y < _islandData.IslandSize && current.y - direction.y >= 0) current.y -= direction.y;
        
            _roadMap[current.x, current.y] = true;
        }
    }

    private void CreateSpawnerRoad(List<Vector2Int> spawnerNodes, Vector2Int[,] roadNodes, int index)
    {
        Vector2Int current = FindNodeIndex(spawnerNodes[index], roadNodes, _islandData.AmountOfRoadNodesBetweenCenterAndEdge * 2 + 3);

        int middleIndex = (_islandData.AmountOfRoadNodesBetweenCenterAndEdge * 2 + 2) / 2;

        _roadMap[roadNodes[current.x, current.y].x, roadNodes[current.x, current.y].y] = true;

        _touchedNodesMap[current.x, current.y] = true;

        int iterator = 0;
        while (current.x != middleIndex && current.y != middleIndex)
        {
            Vector2Int next = GetNextNodeIndex(middleIndex, current.x, current.y);

            MoveRoad(roadNodes[current.x, current.y], roadNodes[current.x + next.x, current.y + next.y]);

            current.x += next.x;
            current.y += next.y;

            _touchedNodesMap[current.x, current.y] = true;

            if (iterator > 20) 
            {
                MoveRoad(roadNodes[current.x, current.y], roadNodes[middleIndex, middleIndex]); 
                current.x = middleIndex;
                current.y = middleIndex;
            }     
            iterator++;
        }

        MoveRoad(roadNodes[current.x, current.y], roadNodes[middleIndex, middleIndex]);
    }

    private Vector2Int FindNodeIndex(Vector2Int position, Vector2Int[,] nodes, int amountOfNodes)
    {
        for (int x = 0; x < amountOfNodes; x++)
        {
            for(int y = 0; y < amountOfNodes; y++)
            {
                if (position.x == nodes[x, y].x && position.y == nodes[x, y].y) return new Vector2Int(x, y);
            }
        }
        return new Vector2Int(0,0);
    }

    private Vector2Int GetNextNodeIndex(int middleIndex, int xIndex, int yIndex)
    {
        int xDifference = NormalizeNumber(middleIndex - xIndex);
        int yDifference = NormalizeNumber(middleIndex - yIndex);

        Vector2Int nextNodeIndex = new Vector2Int(0,0);

        if (xIndex > 0 && xIndex < middleIndex * 2 - 1 && yIndex > 0 && yIndex < middleIndex * 2 - 1)
        {
            if (xDifference == 0) 
            {
                if (Random.Range(0, 100) > 50 && NodeIsUnouched(xIndex - 1, yIndex)) xDifference = -1;
                else if (NodeIsUnouched(xIndex + 1, yIndex)) xDifference = 1;
            }
            else if (yDifference == 0)
            {
                if (Random.Range(0, 100) > 50 && NodeIsUnouched(xIndex, yIndex - 1)) yDifference = -1;
                else if (NodeIsUnouched(xIndex, yIndex - 1)) yDifference = 1;
            }
            else if (Random.Range(0, 100) > 50 && NodeIsUnouched(xIndex, yIndex - yDifference)) yDifference *= -1;
            else if (NodeIsUnouched(xIndex - xDifference, yIndex)) xDifference *= -1;

            return new Vector2Int(xDifference, yDifference);
        }
    
        if (xDifference != 0 && yDifference == 0 && NodeIsUnouched(xIndex + xDifference, yIndex)) nextNodeIndex = new Vector2Int(xDifference, 0);
        else if (yDifference != 0 && xDifference == 0 && NodeIsUnouched(xIndex, yIndex + yDifference)) nextNodeIndex = new Vector2Int(0, yDifference);
        else 
        {
            if (Random.Range(0, 100) > 50 && NodeIsUnouched(xIndex + xDifference, yIndex + yDifference))
            {
                nextNodeIndex = new Vector2Int(xDifference, yDifference);
            }
            else
            {
                if (Random.Range(0, 100) > 50 && NodeIsUnouched(xIndex, yIndex + yDifference)) nextNodeIndex = new Vector2Int(0, yDifference);
                else if (NodeIsUnouched(xIndex + xDifference, yIndex)) nextNodeIndex = new Vector2Int(xDifference, 0);
            }
        } 
        
        return nextNodeIndex;
    }

    private bool NodeIsUnouched(int x, int y) => _touchedNodesMap[x, y] == false;

    private void MoveRoad(Vector2Int currentPos, Vector2Int neededPos)
    {
        while(currentPos != neededPos)
        {
            Vector2Int moveDir = GetMoveDirection(currentPos, neededPos);

            currentPos = currentPos + moveDir;

            _roadMap[currentPos.x, currentPos.y] = true;
        }
    }

    private Vector2Int GetMoveDirection(Vector2Int currentPos, Vector2Int neededPos)
    {
        Vector2Int dirDifference = neededPos - currentPos;

        int xDifference = NormalizeNumber(dirDifference.x);
        int yDifference = NormalizeNumber(dirDifference.y);

        if (xDifference == 0) return new Vector2Int(0, yDifference);
        if (yDifference == 0) return new Vector2Int(xDifference, 0);
        
        if (Random.Range(0, 100) > 50) return new Vector2Int(0, yDifference);
        else return new Vector2Int(xDifference, 0);
    }

    private int NormalizeNumber(int num)
    {
        if (num == 0) return 0;
        return num / Mathf.Abs(num);
    }

    private void AddCenterRoad()
    {
        int centerIndex = Mathf.RoundToInt((_islandData.IslandSize - 1) / 2);

        for (int x = centerIndex - 1; x <= centerIndex + 1; x++)
        {
            for (int y = centerIndex - 1; y <= centerIndex + 1; y++)
            {
                _roadMap[x, y] = true;
            }
        }
    }
}

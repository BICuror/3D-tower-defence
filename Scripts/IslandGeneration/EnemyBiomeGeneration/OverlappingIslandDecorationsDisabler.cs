using UnityEngine;
using Zenject;

public sealed class OverlappingIslandDecorationsDisabler : MonoBehaviour
{
    [Inject] IslandData _islandData;
    [Inject] IslandDecorationContainer _islandDecorationContainer;

    public void DisableOverlappingIslandDecorations(bool[,] enemyBiomeMap, int biomeStage, Vector2Int biomePosition)
    {
        int radius = _islandData.EnemyBiomeStages[biomeStage].EnemyBiomeRadius;

        for (int x = 0; x < radius * 2 + 1; x++)
        {        
            for (int z = 0; z < radius * 2 + 1; z++)
            {   
                if (enemyBiomeMap[x, z])
                {
                    _islandDecorationContainer.SetActiveDecorationsIfInBound(biomePosition.x + x, biomePosition.y + z, false);
                }
            } 
        }
    }
}

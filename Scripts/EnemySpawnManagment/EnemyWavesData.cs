using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWavesData", menuName = "SpawnManagement/EnemyWavesData")]

public class EnemyWavesData : ScriptableObject
{
    [SerializeField] private EnemyWaveGroup[] _possibleGroups;
    public EnemyWaveGroup[] PossibleGroups {get => _possibleGroups;}

    [System.Serializable] public struct Enemy
    {
        [SerializeField] private EnemyData _enemyData;
        public EnemyData Data => _enemyData;

        [SerializeField] private float _wheight;
        public float Weight => _wheight;
    }    
    
    public Enemy[] GetEnemies(EnemyType type)
    {
        Enemy[] enemies = type switch 
        {
            EnemyType.Default => _defaultEnemies,
            EnemyType.Tank => _tankEnemies,
            EnemyType.Ranger => _rangerEnemies,
            EnemyType.Support => _supportEnemies,
            EnemyType.Healer => _healerEnemies,
            EnemyType.Quickie => _quickieEnemies
        };

        return enemies;
    }

    [SerializeField] private Enemy[] _defaultEnemies;
    public Enemy[] DefaultEnemies {get => _defaultEnemies;}

    [SerializeField] private Enemy[] _tankEnemies;
    public Enemy[] TankEnemies {get => _tankEnemies;}

    [SerializeField] private Enemy[] _rangerEnemies;
    public Enemy[] RangerEnemies {get => _rangerEnemies;}

    [SerializeField] private Enemy[] _supportEnemies;
    public Enemy[] SupportEnemies {get => _supportEnemies;}

    [SerializeField] private Enemy[] _healerEnemies;
    public Enemy[] HealerEnemies {get => _healerEnemies;}

    [SerializeField] private Enemy[] _quickieEnemies;
    public Enemy[] QuickieEnemies {get => _quickieEnemies;}
}

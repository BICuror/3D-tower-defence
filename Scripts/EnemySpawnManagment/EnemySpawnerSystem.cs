using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public sealed class EnemySpawnerSystem : MonoBehaviour
{
    [Inject] private WaveManager _waveManager;

    [SerializeField] private float _wheightForWave; 

    private List<EnemySpawner> _spawners = new List<EnemySpawner>();
    [SerializeField] private EnemyWavesData _enemyWavesData;

    public UnityEvent AllEnemiesDied;

    public UnityEvent<GameObject> EnemySpawned;

    public void AddSpawner(EnemySpawner spawner)
    {
        _spawners.Add(spawner);

        spawner.EnemySpawned.AddListener(InvokeEnemySpawned);

        spawner.LastEnemyKilled.AddListener(CheckIfAllEnemiesDied);
    }

    public void RemoveSpawner(EnemySpawner spawner)
    {
        _spawners.Remove(spawner);

        spawner.EnemySpawned.RemoveListener(InvokeEnemySpawned);

        spawner.LastEnemyKilled.RemoveListener(CheckIfAllEnemiesDied);
    }

    private void InvokeEnemySpawned(GameObject spawnedEnemy) => EnemySpawned?.Invoke(spawnedEnemy);

    private void CheckIfAllEnemiesDied()
    {
        for (int i = 0; i < _spawners.Count; i++)
        {
            if (_spawners[i].AllEnemiesDead() == false) return;
        }        

        AllEnemiesDied?.Invoke();
    }

    public void StartWave()
    {
        for (int i = 0; i < _spawners.Count; i++)
        {   
            _spawners[i].StartSpawningEnemies(GetEnemyWave());
        }
    }

    private List<EnemyData> GetEnemyWave()
    {
        List<EnemyData> enemyDatasToSpawn = new List<EnemyData>();

        EnemyWaveGroup currentWaveGroup = _enemyWavesData.PossibleGroups[Random.Range(0, _enemyWavesData.PossibleGroups.Length)];

        float wheightForEnemyTypeInGroup = _wheightForWave / currentWaveGroup.EnemiesInGroup.Length;

        for (int i = 0; i < currentWaveGroup.EnemiesInGroup.Length; i++)
        {
            EnemyType currentEnemyType = currentWaveGroup.EnemiesInGroup[i].Type;

            float weightLeftForGroup = wheightForEnemyTypeInGroup;

            EnemyWavesData.Enemy[] enemiesOfType = _enemyWavesData.GetEnemies(currentEnemyType);

            for (int y = 0; y < currentWaveGroup.EnemiesInGroup[i].Amount; y++)
            {
                for (int wheightIndex = enemiesOfType.Length - 1; wheightIndex > 0; wheightIndex--)
                {
                    if (enemiesOfType[wheightIndex].Weight <= weightLeftForGroup)
                    {
                        enemyDatasToSpawn.Add(enemiesOfType[wheightIndex].Data);

                        weightLeftForGroup -= enemiesOfType[wheightIndex].Weight;

                        break;
                    }
                    
                    enemyDatasToSpawn.Add(enemiesOfType[0].Data);
                }
            }
        }

        return enemyDatasToSpawn;
    }
}

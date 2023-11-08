using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class EnemySpawner : MonoBehaviour
{
    public UnityEvent<GameObject> EnemySpawned;

    private List<GameObject> _spawnedEnemies;

    public UnityEvent LastEnemyKilled;

    private void Awake()
    {
        _spawnedEnemies = new List<GameObject>();

        //LastEnemyKilled.AddListener(StopAllCoroutines);
        //LastEnemyKilled.AddListener(TrySpawnEnemy);
    }

    public bool AllEnemiesDead() => _spawnedEnemies.Count == 0;


    public void StartSpawningEnemies(List<EnemyData> enemies)
    {
        StartCoroutine(WaitToSpawn(enemies));
    }

    private IEnumerator WaitToSpawn(List<EnemyData> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {   
            yield return new WaitForSeconds(1f);

            EnemyHealth spawnedEnemy = EnemyFactory.Instance.CreateEnemy(enemies[i]);

            _spawnedEnemies.Add(spawnedEnemy.gameObject);

            spawnedEnemy.transform.position = transform.position;

            spawnedEnemy.EnemyDeathEvent.AddListener(RemoveEnemy);

            EnemySpawned.Invoke(spawnedEnemy.gameObject);
        }
    }

    private void RemoveEnemy(EnemyHealth enemyHealth)
    {
        _spawnedEnemies.Remove(enemyHealth.gameObject);

        enemyHealth.EnemyDeathEvent.RemoveListener(RemoveEnemy);

        if (_spawnedEnemies.Count == 0) LastEnemyKilled?.Invoke();
    }
}

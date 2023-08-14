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
        FindObjectOfType<EnemySpawnerSystem>().AddSpawner(this);

        _spawnedEnemies = new List<GameObject>();

        //LastEnemyKilled.AddListener(StopAllCoroutines);
        //LastEnemyKilled.AddListener(TrySpawnEnemy);
    }

    private void OnDestroy() => FindObjectOfType<EnemySpawnerSystem>().RemoveSpawner(this);

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

            spawnedEnemy.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            spawnedEnemy.gameObject.GetComponent<AgentLinkMover>().enabled = true;

            spawnedEnemy.DeathEvent.AddListener(RemoveEnemy);

            EnemySpawned.Invoke(spawnedEnemy.gameObject);
        }
    }

    private void RemoveEnemy(EnemyHealth enemyHealth)
    {
        _spawnedEnemies.Remove(enemyHealth.gameObject);

        enemyHealth.DeathEvent.RemoveListener(RemoveEnemy);

        enemyHealth.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        enemyHealth.gameObject.GetComponent<AgentLinkMover>().enabled = false;

        if (_spawnedEnemies.Count == 0) LastEnemyKilled?.Invoke();
    }
}

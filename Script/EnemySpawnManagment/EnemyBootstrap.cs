using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyHealth))]

public sealed class EnemyBootstrap : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;   

    [SerializeField] private NavMeshAgent _navMeshAgent;
    public NavMeshAgent Agent => _navMeshAgent;

    [SerializeField] private EnemyHealth _enemyHealth; 
    public EnemyHealth Health => _enemyHealth;
    
    public void SetEnemyData(EnemyData enemyDataToSet)
    {
        SetEnemyHealthData(enemyDataToSet.HealthData);
        SetEnemyMovmentData(enemyDataToSet.MovmentData);
        SetVisualData(enemyDataToSet);
    }

    private void SetEnemyMovmentData(EnemyMovmentData enemyData)
    {
        _navMeshAgent.speed = enemyData.MovmentSpeed;
    }    

    private void SetEnemyHealthData(EnemyHealthData enemyData)
    {
        _enemyHealth.SetEnemyData(enemyData);
    }    

    private void SetVisualData(EnemyData enemyData)
    {
        _meshFilter.sharedMesh = enemyData.GetMesh();
        _meshRenderer.sharedMaterial = enemyData.GetMaterial();

    }
}

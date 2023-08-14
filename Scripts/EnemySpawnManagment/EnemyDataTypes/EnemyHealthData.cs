using UnityEngine;

[CreateAssetMenu(fileName = "EnemyHealthData", menuName = "EnemyDatas/EnemyHealthData")]

public sealed class EnemyHealthData : ScriptableObject 
{
    [SerializeField] private float _maxHealth;
    public float MaxHealth => _maxHealth;
}
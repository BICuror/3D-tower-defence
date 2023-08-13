using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaveGroup", menuName = "Block Tower Defence/EnemyWaveGroup")]

public sealed class EnemyWaveGroup : ScriptableObject 
{    
    [SerializeField] private EnemyInGroup[] _enemiesInGroup;
    public EnemyInGroup[] EnemiesInGroup => _enemiesInGroup;
    
    [System.Serializable] public struct EnemyInGroup
    {
        [SerializeField] private EnemyType _type; 
        public EnemyType Type => _type;

        [SerializeField] private int _amount;
        public int Amount => _amount;
    }
}

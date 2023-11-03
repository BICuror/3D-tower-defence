using UnityEngine;

public sealed class MortarTower : MonoBehaviour
{
    [Header("StatSettings")]
    [SerializeField] private float _explotionDamage;
    [SerializeField] private float _contactDamage;
    [SerializeField] private float _explotionRadius;
    
    [Header("Links")]
    [SerializeField] private EnemyAreaScaner _enemyAreaScaner;
    [SerializeField] private MortarGrenade _grenadePrefab;
    [SerializeField] private ApplyEffectContainer _applyEffectContainer;

    private ObjectPool<MortarGrenade> _grenadeObjectPool; 

    private void Start()
    {   
        _grenadeObjectPool = new ObjectPool<MortarGrenade>(_grenadePrefab, 2);

        TaskCycle buildingTaskCycle = GetComponent<TaskCycle>();

        buildingTaskCycle.ShouldWorkDelegate = ShouldWorkDelegate;

        buildingTaskCycle.TaskPerformed.AddListener(Shoot);
    }

    private bool ShouldWorkDelegate() => _enemyAreaScaner.Empty() == false;

    private void Shoot()
    {
        MortarGrenade currentGrenade = _grenadeObjectPool.GetNextPooledObject();
        
        currentGrenade.transform.position = transform.position;

        currentGrenade.SetContactDamage(_contactDamage);
        currentGrenade.SetExplotionDamage(_explotionDamage);
        currentGrenade.SetExplotionRaduis(_explotionRadius);

        currentGrenade.SetEffects(_applyEffectContainer.GetApplyEffects());

        currentGrenade.Launch(_enemyAreaScaner.GetRandomEnemy().transform.position);
    }
}

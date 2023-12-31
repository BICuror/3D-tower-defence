using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : EntityHealth
{
    public UnityEvent<EnemyHealth> EnemyDeathEvent; 

    private float _attackMultipluer;

    public void SetEnemyData(EnemyHealthData enemyData)
    {
        _maxHealth = enemyData.MaxHealth;

        _incomingDamageMultipluer = enemyData.IncomingDamageMultipluer;

        HealFully();

        EnableHealthBar();
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.TryGetComponent(out BuildingHealth buildingHealth))
        {
            buildingHealth.GetHurt(GetCurrentHealth());

            Die();
        }    
    }

    public override void Die()
    {
        DeathEvent.Invoke(gameObject);

        EnemyDeathEvent.Invoke(this);

        gameObject.SetActive(false);
    }
}

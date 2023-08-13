using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : EntityHealth
{
    public UnityEvent<EnemyHealth> DeathEvent; 

    [SerializeField] private float _damageDecreaseSpeed = 0.1f;

    public void SetEnemyData(EnemyHealthData enemyData)
    {
        _maxHealth = enemyData.MaxHealth;

        HealFully();
    }

    private void OnCollisionStay(Collision other) 
    {
        if (other.gameObject.TryGetComponent(out BuildingHealth buildingHealth))
        {
            GetHurt(_damageDecreaseSpeed);
            buildingHealth.GetHurt(_damageDecreaseSpeed);
        }    
    }

    public override void Die()
    {
        gameObject.SetActive(false);

        DestroyEvent?.Invoke(gameObject);

        DeathEvent?.Invoke(this);
    }
}

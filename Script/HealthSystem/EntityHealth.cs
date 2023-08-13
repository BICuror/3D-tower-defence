using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] protected float _maxHealth;
    private float _currentHealth;

    public UnityEvent HealthChanged;
    
    public UnityEvent<GameObject> DestroyEvent; 

    [SerializeField] protected HealthBar _healthBar;

    private void Awake() => _currentHealth = _maxHealth;

    public void HealFully() => Heal(_maxHealth - _currentHealth);
    public void EnableHealthBar()
    {
        _healthBar.gameObject.SetActive(true);

        _healthBar.UpdateValue(GetHealthPrcentage());
    }

    public void DisableHealthBar() => _healthBar.gameObject.SetActive(false);

    public float GetMaxHealth() => _maxHealth;
    public float GetHealthPrcentage() => _currentHealth / _maxHealth;
    public void GetHurt(float damage)
    {
        _currentHealth -= damage;

        HealthChanged?.Invoke();

        if (_currentHealth <= 0) 
        {
            Die();
        }
        else
        {
            _healthBar.UpdateValue(GetHealthPrcentage());
        }
    }
    
    public void Heal(float healAmount)
    {
        if (_currentHealth + healAmount <= _maxHealth)
        {
            _currentHealth += healAmount;
        }
        else
        { 
            _currentHealth = _maxHealth;
        }

        HealthChanged?.Invoke();
        _healthBar.UpdateValue(GetHealthPrcentage());
    }

    public virtual void Die()
    {
        DestroyEvent?.Invoke(gameObject);
        
        Destroy(gameObject);
    }
}   

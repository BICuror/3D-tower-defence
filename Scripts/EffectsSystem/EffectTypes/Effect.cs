using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public abstract class Effect : ScriptableObject 
{
    [Header("UI VisualSettings")]

    [SerializeField] private Material _uIMaterial;
    public Material UIMaterial {get => _uIMaterial;}

    [Header("EffectVisualSettings")]
    [SerializeField] private VisualEffectPoolObjectHandler _effectPrefab;

    [SerializeField] private int _initialEffectPoolSize;

    protected ObjectPool<VisualEffectPoolObjectHandler> _visualEffectsPool; 

    public void TryInstaitiateVisualEffectsPool()
    {
        if (_visualEffectsPool == null)
        {
            _visualEffectsPool = new ObjectPool<VisualEffectPoolObjectHandler>(_effectPrefab, _initialEffectPoolSize);
            
            SceneManager.sceneLoaded += DestroyVisualEffectsPool;
        }
    }   

    private void DestroyVisualEffectsPool(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.sceneLoaded -= DestroyVisualEffectsPool;

        _visualEffectsPool = null;
    }

    public VisualEffectPoolObjectHandler GetEffectObject() => _visualEffectsPool.GetNextPooledObject();

    public abstract EffectType GetEffectType();

    public abstract bool CanBeApplied(EntityComponentsContainer componentsContainer);
    
    public abstract void ApplyToEntity(EntityComponentsContainer componentsContainer);

    public abstract void RemoveFromEntity(EntityComponentsContainer componentsContainer);  
}

public enum EffectType
{
    Permanent,
    PermanentOverTime,
    RemoveOverTime,
    RemoveOverTicks
}

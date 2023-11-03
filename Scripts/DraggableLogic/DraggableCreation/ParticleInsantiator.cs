using UnityEngine;

public sealed class ParticleInsantiator : MonoBehaviour
{
    [SerializeField] private VisualEffectPoolObjectHandler _particlePrefab;

    private ObjectPool<VisualEffectPoolObjectHandler> _effectPool;

    private void Awake()
    {
        _effectPool = new ObjectPool<VisualEffectPoolObjectHandler>(_particlePrefab, 2);
    } 

    public void InstantiateParticle(GameObject placedObject)
    {
        Transform visualEffectObject = _effectPool.GetNextPooledObject().transform;

        visualEffectObject.position = placedObject.transform.position;

        visualEffectObject.rotation = placedObject.transform.rotation;
    }
}

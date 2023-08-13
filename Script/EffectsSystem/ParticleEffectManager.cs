using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.VFX;

public sealed class ParticleEffectManager : MonoBehaviour
{
    [SerializeField] private EntityEffectManager _entityEffectManager;

    private Dictionary<Effect, VisualEffectPoolObjectHandler> _appliedEffects = new Dictionary<Effect, VisualEffectPoolObjectHandler>();

    public void ApplyEffect(Effect effect)
    {
        if (_appliedEffects.ContainsKey(effect) == false)
        {        
            VisualEffectPoolObjectHandler visualEffect = effect.GetEffectObject();
    
            _appliedEffects.Add(effect, visualEffect);

            visualEffect.transform.SetParent(transform);

            visualEffect.transform.localPosition = Vector3.zero;

            visualEffect.transform.localRotation = Quaternion.identity;
        }
    }

    public void RemoveEffect(Effect effect)
    {
        if (_entityEffectManager.HasEffect(effect) == false)
        {
            VisualEffectPoolObjectHandler visualEffect = _appliedEffects[effect];

            _appliedEffects.Remove(effect);

            visualEffect.transform.SetParent(null);

            visualEffect.Stop();
        }
    }
}

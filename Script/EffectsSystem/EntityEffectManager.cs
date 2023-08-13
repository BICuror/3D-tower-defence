using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class EntityEffectManager : MonoBehaviour
{
    private EntityComponentsContainer _entityComponentsContainer;

    private List<Effect> _appliedEffects;

    public UnityEvent<Effect> EffectApplied;

    public UnityEvent<Effect> EffectRemoved;
    
    public List<Effect> GetAllEffecs() => _appliedEffects;

    public bool HasEffect(Effect effect) => _appliedEffects.Contains(effect);

    private void Start()
    {
        _appliedEffects = new List<Effect>();

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        EntityHealth health = GetComponent<EntityHealth>();
        TaskCycle taskCycle = GetComponent<TaskCycle>();

        _entityComponentsContainer = new EntityComponentsContainer(health, taskCycle, agent);
    }
    
    public void ApplyEffect(Effect effectToApply)
    {
        if (effectToApply.CanBeApplied(_entityComponentsContainer))
        {
            _appliedEffects.Add(effectToApply);

            EffectApplied?.Invoke(effectToApply);

            switch (effectToApply.GetEffectType())
            {
                case EffectType.Permanent: ApplyEffectPermanently(effectToApply as PermanentEffect); break;
                case EffectType.PermanentOverTime: ApplyEffectOverTimePermanently(effectToApply as PermanentOverTimeEffect); break;
                case EffectType.RemoveOverTime: ApplyEffectAndRemoveAfterTime(effectToApply as RemoveOverTimeEffect); break;
                case EffectType.RemoveOverTicks: ApplyEffectOverTicks(effectToApply as RemoveOverTicksEffect); break;
            }
        }
    }

    public void RemoveAllEffects()
    {
        for (int i = 0; i < _appliedEffects.Count; i++)
        {
            Effect effectToRemove = _appliedEffects[i];

            _appliedEffects.Remove(effectToRemove);

            EffectRemoved?.Invoke(effectToRemove);

            effectToRemove.RemoveFromEntity(_entityComponentsContainer);
        }
    }
    
    public void RemoveEffect(Effect effectToRemove)
    {
        if (HasEffect(effectToRemove))
        {
            _appliedEffects.Remove(effectToRemove);

            EffectRemoved?.Invoke(effectToRemove);

            effectToRemove.RemoveFromEntity(_entityComponentsContainer);
        }
    }

    #region EffectOverTicks
    public void ApplyEffectOverTicks(RemoveOverTicksEffect effect) => StartCoroutine(ApplyEffectOverTime(effect));   

    private IEnumerator ApplyEffectOverTime(RemoveOverTicksEffect effect)
    {
        effect.ApplyToEntity(_entityComponentsContainer);

        YieldInstruction instruction = new WaitForSeconds(effect.TickDuration);

        for (int i = 0; i < effect.TicksAmount; i++)
        {
            yield return instruction;

            effect.ApplyTickEffectToEntity(_entityComponentsContainer);
        }

        RemoveEffect(effect);
    }    
    #endregion
    
    #region PermanentEffectOverTime
    public void ApplyEffectOverTimePermanently(PermanentOverTimeEffect effect) => StartCoroutine(PermemantlyApplyEffectOverTime(effect));   

    private IEnumerator PermemantlyApplyEffectOverTime(PermanentOverTimeEffect effect)
    {
        effect.ApplyToEntity(_entityComponentsContainer);

        YieldInstruction instruction = new WaitForSeconds(effect.TickDuration);

        while (true)
        {
            yield return instruction;

            effect.ApplyTickEffectToEntity(_entityComponentsContainer);
        }
    }    
    #endregion

    #region ApplyAndRemoveOverTime
    public void ApplyEffectAndRemoveAfterTime(RemoveOverTimeEffect effect) => StartCoroutine(ApplyAndRemove(effect));    

    private IEnumerator ApplyAndRemove(RemoveOverTimeEffect effect)
    {
        effect.ApplyToEntity(_entityComponentsContainer);

        yield return new WaitForSeconds(effect.EffecDuration);
        
        RemoveEffect(effect);
    }
    #endregion

    #region ApplyEffectPermanently
    public void ApplyEffectPermanently(PermanentEffect effect)
    {
        effect.ApplyToEntity(_entityComponentsContainer);
    }
    #endregion
}
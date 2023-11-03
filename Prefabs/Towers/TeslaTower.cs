using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TeslaTower : MonoBehaviour
{
    [SerializeField] private int _beamDamage;

    [SerializeField] private BeamSystem _beamSystem;

    [SerializeField] private ApplyEffectContainer _applyEffectContainer;

    [SerializeField] private EnemyAreaScaner _enemyAreaScaner;

    private void Start()
    {
        TaskCycle buildingTaskCycle = GetComponent<TaskCycle>();

        buildingTaskCycle.ShouldWorkDelegate = ShouldWorkDelegate;

        buildingTaskCycle.TaskPerformed.AddListener(Beam);
    }

    private bool ShouldWorkDelegate() => _enemyAreaScaner.Empty() == false;

    private void Beam()
    {
        EnemyHealth enemyHealth = _enemyAreaScaner.GetRandomEnemy();
        
        enemyHealth.GetHurt(_beamDamage);

        _beamSystem.StartBeamTranstionToPosition(enemyHealth.transform);

        if (enemyHealth.IsAlive()) ApplyEffectsToEnemy(enemyHealth);
    }

    private void ApplyEffectsToEnemy(EnemyHealth enemyHealth)
    {
        List<Effect> effectsToApply = _applyEffectContainer.GetApplyEffects();

        if (effectsToApply.Count > 0)
        { 
            EnemyEffectManager enemyEffectManager = enemyHealth.gameObject.GetComponent<EnemyEffectManager>();

            for (int i = 0; i < effectsToApply.Count; i++)
            {
                if (enemyHealth.IsAlive()) enemyEffectManager.ApplyEffect(effectsToApply[i]);
            }
        }
    }
}

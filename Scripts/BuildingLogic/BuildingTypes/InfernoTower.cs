using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InfernoTower : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _damageAccseleration;

    [SerializeField] private float _baseDamage;
    private float _currentDamage;
    
    [SerializeField] private float _additionalDamagePerEffect;

    [Header("Links")]    
    [SerializeField] private EnemyAreaScaner _enemyAreaScaner;

    [SerializeField] private ApplyEffectContainer _applyEffectContainer;
    
    [SerializeField] private BeamSystem _beamSystem;

    private BuildingTaskCycle _buildingTaskCycle;

    private Building _building;

    private EnemyHealth _currentEnemy;

    private void Start()
    {
        _buildingTaskCycle = GetComponent<BuildingTaskCycle>();
        _buildingTaskCycle.ShouldWorkDelegate = ShouldWork;
        _buildingTaskCycle.TaskPerformed.AddListener(Beam);

        _beamSystem.BeamConnected.AddListener(_buildingTaskCycle.StartCycle);

        _building = GetComponent<Building>();
        _building.PickedUp.AddListener(_beamSystem.DisableBeam);
        _building.PickedUp.AddListener(ClearEnemy);
        _building.BuildCompleted.RemoveListener(_buildingTaskCycle.StartCycle);
        _building.BuildCompleted.AddListener(_beamSystem.SetBeamPositionToSource);
        _building.BuildCompleted.AddListener(TrySettingFirstEnemy);

        _enemyAreaScaner.EnemyEnteredArea.AddListener(TrySettingNewEnemy);

        _enemyAreaScaner.EnemyExitedArea.AddListener(TryRemovingEnemy);
    }  

    private void ClearEnemy() => _currentEnemy = null;
    
    private bool ShouldWork() => _enemyAreaScaner.Empty() == false;

    private void TrySettingFirstEnemy()
    {
        if (ShouldWork())
        {
            TrySettingNewEnemy(_enemyAreaScaner.GetFirstEnemy());
        }
    }

    private void TrySettingNewEnemy(EnemyHealth enemy)
    {
        if (_currentEnemy == null && _building.IsBuilt())
        {
            _currentEnemy = enemy;

            _currentDamage = _baseDamage;

            _beamSystem.StartBeamTranstionToPosition(enemy.transform);
        }
    }

    private void TryRemovingEnemy(EnemyHealth enemy)
    {
        if (_currentEnemy == enemy)
        {
            _currentEnemy = null;

            _buildingTaskCycle.StopCycle();

            if (_enemyAreaScaner.Empty() == false) 
            {
                TrySettingNewEnemy(_enemyAreaScaner.GetFirstEnemy());
            }
            else
            {
                _beamSystem.ReturnBeam();
            }
        }
    }

    private void Beam()
    {
        _currentDamage *= _damageAccseleration;
        _currentEnemy.GetHurt(_currentDamage + _additionalDamagePerEffect * _applyEffectContainer.GetAmountOfEffects());

        if (_currentEnemy.IsAlive()) ApplyEffectsToEnemy();
    } 

    private void ApplyEffectsToEnemy()
    {
        List<Effect> effectsToApply = _applyEffectContainer.GetApplyEffects();

        if (effectsToApply.Count > 0)
        { 
            EnemyEffectManager enemyEffectManager = _currentEnemy.gameObject.GetComponent<EnemyEffectManager>();

            for (int i = 0; i < effectsToApply.Count; i++)
            {
                if (_currentEnemy.IsAlive()) enemyEffectManager.ApplyEffect(effectsToApply[i]);
            }
        }
    }
}

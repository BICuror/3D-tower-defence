using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HealingTower : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _healAmount;

    [Header("Links")]
    [SerializeField] private BuildingHealthAreaScaner _buildingHealthAreaScaner;

    [SerializeField] private BeamSystem _beamSystem;

    private EntityHealth _currentBuilding;

    private BuildingTaskCycle _buildingTaskCycle;

    private Building _building;

    private void Start()
    {
        _buildingTaskCycle = GetComponent<BuildingTaskCycle>();
        _buildingTaskCycle.ShouldWorkDelegate = ShouldWork;
        _buildingTaskCycle.TaskPerformed.AddListener(HealBuilding);
        

        _buildingHealthAreaScaner.HealthComponentAdded.AddListener(TrySettingNewBuilding);
        _buildingHealthAreaScaner.HealthComponentRemoved.AddListener(TryRemovingBuilding);

        _buildingHealthAreaScaner.HealthComponentAdded.AddListener(SubscribeToBuilding);
        _buildingHealthAreaScaner.HealthComponentRemoved.AddListener(UnsubscribeToBuilding);

        _building = GetComponent<Building>();
        _building.PickedUp.AddListener(_beamSystem.DisableBeam);
        _building.PickedUp.AddListener(ClearBuilding);
        _building.BuildCompleted.AddListener(_beamSystem.SetBeamPositionToSource);
    }

    private void ClearBuilding() => _currentBuilding = null;

    private void SubscribeToBuilding(EntityHealth building) => building.Damaged.AddListener(TryToHealOnHurt);
    private void UnsubscribeToBuilding(EntityHealth building) => building.Damaged.RemoveListener(TryToHealOnHurt);

    private bool ShouldWork() 
    {
        if (_buildingHealthAreaScaner.IsEmpty()) return false;

        IReadOnlyList<EntityHealth> buildings = _buildingHealthAreaScaner.GetHealthComponentsList();

        for (int i = 0; i < buildings.Count; i++)
        {
            if (buildings[i].GetHealthPrcentage() < 1f) return true; 
        }

        if (_currentBuilding != null) 
        {
            _currentBuilding = null;
            _beamSystem.ReturnBeam(); 
        }
        return false;
    }

    private void TrySettingNewBuilding(EntityHealth building)
    {
        if (_building.IsBuilt() && _currentBuilding == null && ShouldWork())
        {
            SetNewBuilding();
        }
    }

    private void TryRemovingBuilding(EntityHealth building)
    {
        if (_currentBuilding == (building as BuildingHealth))
        {
            _currentBuilding = null;

            if (ShouldWork()) SetNewBuilding();
            else _beamSystem.ReturnBeam();
        }
    }

    private void TryToHealOnHurt()
    {
        if (_currentBuilding == null && _building.IsBuilt()) 
        {
            SetNewBuilding();       
        }
    }

    private void SetNewBuilding()
    {
        float leastHp = 1f;

        int index = 0;

        IReadOnlyList<EntityHealth> buildings = _buildingHealthAreaScaner.GetHealthComponentsList();

        for (int i = 0; i < buildings.Count; i++)
        {
            float buildingHealth = buildings[i].GetHealthPrcentage();

            if (buildingHealth < leastHp)
            {
                index = i;
                leastHp = buildingHealth;
            }
        }

        _currentBuilding = buildings[index];

        _buildingTaskCycle.StartCycle();

        _beamSystem.StartBeamTranstionToPosition(_currentBuilding.transform);
    }

    private void HealBuilding()
    {
        _currentBuilding.Heal(_healAmount);

        if (_currentBuilding.GetHealthPrcentage() == 1f)
        {
            _currentBuilding = null;

            if (ShouldWork()) 
            {
                SetNewBuilding();
            }
            else
            {
                _beamSystem.ReturnBeam();
            }
        }
    }
}

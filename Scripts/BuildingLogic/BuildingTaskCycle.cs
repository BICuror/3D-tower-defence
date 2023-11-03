using UnityEngine;

public sealed class BuildingTaskCycle : TaskCycle
{
    private Building _building;

    private void Start()
    {
        _building = GetComponent<Building>();
        
        _building.PickedUp.AddListener(StopCycle);

        _building.BuildCompleted.AddListener(StartCycle);
    }

    public override bool CanWork() => _building.IsBuilt(); 
}

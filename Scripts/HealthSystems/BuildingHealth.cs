using UnityEngine;

public sealed class BuildingHealth : EntityHealth
{
    private void Start()
    {
        HealthChanged.AddListener(CheckToHideHealthBar);

        _healthBar.gameObject.SetActive(false);

        Building building = GetComponent<Building>();

        building.PickedUp.AddListener(DisableHealthBar);
        building.Placed.AddListener(CheckToHideHealthBar);
    }

    private void CheckToHideHealthBar()
    {
        float value = GetHealthPrcentage();

        if (value == 1 && _healthBar.gameObject.activeSelf == true) DisableHealthBar();
        else if (_healthBar.gameObject.activeSelf == false && value != 1) EnableHealthBar();
    }
}

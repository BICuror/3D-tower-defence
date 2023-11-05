using UnityEngine;

[RequireComponent(typeof(EntityHealth))]

public sealed class HitShaker : Shaker
{
    private void Awake() => GetComponent<EntityHealth>().Damaged.AddListener(Shake);
}

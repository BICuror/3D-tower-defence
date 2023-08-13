using UnityEngine;

[CreateAssetMenu(fileName = "PoisonEffect", menuName = "Effect/Poison")]

public sealed class PoisonEffect : PermanentOverTimeEffect
{
    [SerializeField] private float _damage;

    public override bool CanBeApplied(EntityComponentsContainer componentsContainer) => componentsContainer.HasHealth();

    public override void ApplyToEntity(EntityComponentsContainer componentsContainer)
    {
        componentsContainer.Health.GetHurt(_damage);
    }

    public override void ApplyTickEffectToEntity(EntityComponentsContainer componentsContainer)
    {
        componentsContainer.Health.GetHurt(_damage);
    }

    public override void RemoveFromEntity(EntityComponentsContainer componentsContainer)
    {
        componentsContainer.Health.GetHurt(_damage);
    }
}

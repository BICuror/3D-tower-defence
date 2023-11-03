using UnityEngine;
using Zenject;

public sealed class EnemyBiomesGenerationInstaller : MonoInstaller
{
    [SerializeField] private EnemyBiomeGenerator _enemyBiomeGenerator;
    [SerializeField] private EnemyBiomeMeshGenerator _enemyBiomeMeshGenerator; 

    public override void InstallBindings()
    {
        Container.Bind<EnemyBiomeContainer>().AsSingle().NonLazy();
        Container.Bind<EnemyBiomeGenerator>().FromInstance(_enemyBiomeGenerator).AsSingle().NonLazy();
    }
}
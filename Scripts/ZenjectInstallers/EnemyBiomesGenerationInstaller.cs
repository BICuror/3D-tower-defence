using UnityEngine;
using Zenject;

public sealed class EnemyBiomesGenerationInstaller : MonoInstaller
{
    [SerializeField] private EnemyBiomeGenerator _enemyBiomeGenerator;
    [SerializeField] private EnemyBiomeMeshGenerator _enemyBiomeMeshGenerator; 
    [SerializeField] private OverlappingIslandDecorationsDisabler _overlappingIslandDecorationsDisabler;
    [SerializeField] private EnemyBiomeMapToGridConverter _enemyBiomeMapToGridConverter;
    [SerializeField] private EnemyBiomeMapGenerator _enemyBiomeMapGenerator;

    public override void InstallBindings()
    {
        Container.Bind<EnemyBiomeContainer>().AsSingle().NonLazy();

        Container.Bind<EnemyBiomeGenerator>().FromInstance(_enemyBiomeGenerator).AsSingle().NonLazy();
        Container.Bind<EnemyBiomeMeshGenerator>().FromInstance(_enemyBiomeMeshGenerator).AsSingle().NonLazy();
        Container.Bind<OverlappingIslandDecorationsDisabler>().FromInstance(_overlappingIslandDecorationsDisabler).AsSingle().NonLazy();
        Container.Bind<EnemyBiomeMapToGridConverter>().FromInstance(_enemyBiomeMapToGridConverter).AsSingle().NonLazy();
        Container.Bind<EnemyBiomeMapGenerator>().FromInstance(_enemyBiomeMapGenerator).AsSingle().NonLazy();
    }
}

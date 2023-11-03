using UnityEngine;
using Zenject;

public sealed class TerrainGenerationInstaller : MonoInstaller
{
    [SerializeField] private IslandGenerator _islandGenerator;
    [SerializeField] private IslandDecorationGenerator _decorationGenerator;
    [SerializeField] private EnemyBiomeGenerator _enemyBiomeGenerator;
    [SerializeField] private ResourceSourceGenerator _resourceSourceGenerator;
    [SerializeField] private EnviromentCreator _enviromentCreator;
    [SerializeField] private WaveManager _waveManager; 

    public override void InstallBindings()
    {
        Container.Bind<TextureManager>().AsSingle().NonLazy();
        Container.Bind<BiomeMapGenerator>().AsSingle().NonLazy();
        Container.Bind<HeightMapGenerator>().AsSingle().NonLazy();

        Container.Bind<IslandDecorationGenerator>().FromInstance(_decorationGenerator).AsSingle().NonLazy();
        Container.Bind<ResourceSourceGenerator>().FromInstance(_resourceSourceGenerator).AsSingle().NonLazy();
        Container.Bind<EnviromentCreator>().FromInstance(_enviromentCreator).AsSingle().NonLazy();
        Container.Bind<WaveManager>().FromInstance(_waveManager).AsSingle().NonLazy();

        Container.Bind<IslandGenerator>().FromInstance(_islandGenerator).AsSingle().NonLazy();
    }
}

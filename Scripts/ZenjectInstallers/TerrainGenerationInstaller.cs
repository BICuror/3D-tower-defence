using UnityEngine;
using Zenject;

public sealed class TerrainGenerationInstaller : MonoInstaller
{
    [SerializeField] private IslandGenerator _islandGenerator;
    [SerializeField] private IslandDecorationGenerator _decorationGenerator;
    [SerializeField] private EnemyBiomeGenerator _enemyBiomeGenerator;
    [SerializeField] private EnviromentCreator _enviromentCreator;
    [SerializeField] private WaveManager _waveManager; 
    [SerializeField] private IslandTerrainMeshCreator _islandTerrainMeshCreator;
    [SerializeField] private IslandDecorationContainer _islandDecorationContainer;

    public override void InstallBindings()
    {
        Container.Bind<TextureManager>().AsSingle().NonLazy();
        Container.Bind<BiomeMapGenerator>().AsSingle().NonLazy();
        Container.Bind<HeightMapGenerator>().AsSingle().NonLazy();

        Container.Bind<IslandDecorationGenerator>().FromInstance(_decorationGenerator).AsSingle().NonLazy();
        Container.Bind<IslandDecorationContainer>().FromInstance(_islandDecorationContainer).AsSingle().NonLazy();
        Container.Bind<IslandTerrainMeshCreator>().FromInstance(_islandTerrainMeshCreator).AsSingle().NonLazy();
        Container.Bind<EnviromentCreator>().FromInstance(_enviromentCreator).AsSingle().NonLazy();
        Container.Bind<WaveManager>().FromInstance(_waveManager).AsSingle().NonLazy();

        Container.Bind<IslandGenerator>().FromInstance(_islandGenerator).AsSingle().NonLazy();
    }
}

using UnityEngine;
using Zenject;

public class RoadGenerationInstaller : MonoInstaller
{
    [SerializeField] private RoadMapGenerator _roadMapGenerator;
    [SerializeField] private RoadGenerator _roadGenerator;
    [SerializeField] private NavMeshLinksGenerator _navMeshLinksGenerator;

    public override void InstallBindings()
    {
        Container.Bind<RoadMapGenerator>().FromInstance(_roadMapGenerator).AsSingle().NonLazy();
        Container.Bind<RoadGenerator>().FromInstance(_roadGenerator).AsSingle().NonLazy();
        Container.Bind<NavMeshLinksGenerator>().FromInstance(_navMeshLinksGenerator).AsSingle().NonLazy();
        
        Container.Bind<RoadNodeGenerator>().AsSingle().NonLazy();
        Container.Bind<SpawnerRoadNodeGenerator>().AsSingle().NonLazy();
    }
}

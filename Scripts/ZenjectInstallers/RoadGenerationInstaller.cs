using UnityEngine;
using Zenject;

public class RoadGenerationInstaller : MonoInstaller
{
    [SerializeField] private RoadMapGenerator _roadMapGenerator;
    [SerializeField] private RoadNodeGenerator _roadNodeGenerator;
    [SerializeField] private RoadGenerator _roadGenerator;

    public override void InstallBindings()
    {
        Container.Bind<RoadMapGenerator>().FromInstance(_roadMapGenerator).AsSingle().NonLazy();
        Container.Bind<RoadNodeGenerator>().AsSingle().NonLazy();
        Container.Bind<SpawnerRoadNodeGenerator>().AsSingle().NonLazy();
        Container.Bind<RoadGenerator>().FromInstance(_roadGenerator).AsSingle().NonLazy();
    }
}

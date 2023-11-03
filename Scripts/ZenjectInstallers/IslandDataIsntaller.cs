using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Installers/IslandDataInstaller")]

public sealed class IslandDataIsntaller : ScriptableObjectInstaller<IslandDataIsntaller> 
{
    [SerializeField] private IslandData _islandData;

    public override void InstallBindings()
    {
        Container.Bind<IslandData>().FromInstance(_islandData).AsSingle().NonLazy();
    }
}

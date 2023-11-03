using UnityEngine;
using Zenject;

public sealed class IslandTerrainMeshCreator : MonoBehaviour
{
    [Inject] private TextureManager _textureManager;

    [SerializeField] private TerrainSetter _islandTerrainSetter;

    public void CreateMesh(BlockGrid blockGrid)
    {
        IslandTerrainMeshGenerator meshGenerator = new IslandTerrainMeshGenerator();

        meshGenerator.SetupGenerator(blockGrid, _textureManager);

        Mesh mesh = meshGenerator.GetMesh();

        _islandTerrainSetter.SetMesh(mesh);
    }
}

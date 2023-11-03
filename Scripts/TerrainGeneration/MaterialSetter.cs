using UnityEngine;
using Zenject;

public sealed class MaterialSetter : MonoBehaviour
{  
    [Inject] IslandData _islandData;

    [SerializeField] private Material[] _enivromentMaterials;
    [SerializeField] private Material[] _buildingMaterials;

    private void Start() => UpdateMaterialsMaterial();

    public void UpdateMaterialsMaterial()
    {
        SetEnivromentBaseMap();
        SetBuildingsBaseMap();
    }

    public void SetEnivromentBaseMap()
    {
        for (int i = 0; i < _enivromentMaterials.Length; i++)
        {
            _enivromentMaterials[i].SetTexture("_BaseMap", _islandData.EniviromentTexture);
        }
    }

    public void SetBuildingsBaseMap()
    {
        for (int i = 0; i < _buildingMaterials.Length; i++)
        {
            _buildingMaterials[i].SetTexture("_BaseMap", _islandData.BuildingsTexture);
        }
    }
}

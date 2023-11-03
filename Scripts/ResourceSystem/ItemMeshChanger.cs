using UnityEngine;

public sealed class ItemMeshChanger : MonoBehaviour
{
    [SerializeField] private Mesh[] _meshes;

    [SerializeField] private MeshFilter _meshFilter;
    
    private void Awake() => _meshFilter.sharedMesh = _meshes[Random.Range(0, _meshes.Length)];
}

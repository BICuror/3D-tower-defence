using UnityEngine;

public sealed class EnemyBiomeDecorationMaterialChanger : MonoBehaviour
{
    [SerializeField] private Material _transitionMaterial;

    [SerializeField] private Material _baseMaterial; 

    [SerializeField] private DecorationContainer _decorationContainer;

    [SerializeField] private TerrainAnimator _terrainAnimator;

    [SerializeField] private MeshRenderer _spawnerMesh;
    private Material _baseSpawnerMaterial;

    private void Awake()
    {
        _terrainAnimator.AnitmationStarted.AddListener(ApplyTransitionMaterial);
        _terrainAnimator.AnimationEnded.AddListener(ApplyBaseMaterial);

        _baseSpawnerMaterial = _spawnerMesh.sharedMaterial;
    }

    public void ApplyTransitionMaterial()
    {
        _decorationContainer.ApplyMaterialToAllDecorations(_terrainAnimator.TransitionMaterial);
    
        _spawnerMesh.sharedMaterial = _terrainAnimator.TransitionMaterial;
    }  

    private void ApplyBaseMaterial()
    {
        _decorationContainer.ApplyMaterialToAllDecorations(_baseMaterial);

        _spawnerMesh.sharedMaterial = _baseSpawnerMaterial;
    }
}

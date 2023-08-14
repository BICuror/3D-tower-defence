using UnityEngine;

[CreateAssetMenu(fileName = "LayerSetting", menuName = "Block Tower Defence/LayerSetting")]

public sealed class LayerSetting : ScriptableObject 
{
    [SerializeField] private LayerMask _layerMask;
    public LayerMask GetLayerMask() => _layerMask;
}

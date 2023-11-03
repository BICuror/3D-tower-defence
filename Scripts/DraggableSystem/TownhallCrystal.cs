using UnityEngine;

public sealed class TownhallCrystal : DraggableObject
{
    [SerializeField] private LayerSetting _townhallLayerMaskSettings;

    public override bool CanBePlacedAt(float x, float z, LayerSetting layerSetting)
    {
        RaycastHit[] hits = Physics.RaycastAll(new Vector3(x, 10000f, z), Vector3.down, Mathf.Infinity, layerSetting.GetLayerMask());

        if (hits.Length == 0) return true;
        else if (hits.Length == 1 && hits[0].collider.gameObject == gameObject) return true;
        else if (hits.Length >= 2 && (IsInLayerMask(hits[0].collider.gameObject, _townhallLayerMaskSettings) || IsInLayerMask(hits[1].collider.gameObject, _townhallLayerMaskSettings))) return true;
        else return false;
    }
    
    private bool IsInLayerMask(GameObject layerObject, LayerSetting layerSetting)
    {
        return ((layerSetting.GetLayerMask() & (1 << layerObject.layer)) != 0);
    }
    
}

public interface IDraggable 
{
    void PickUp();
    
    void Place();  

    bool IsDraggable();   

    bool CanBePlacedAt(float x, float z, LayerSetting layerSetting);
}

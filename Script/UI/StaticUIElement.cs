using UnityEngine;

public sealed class StaticUIElement : IngameUIElement
{
    private void Start()
    {
        _cameraRotationController.CameraChangedPosition.AddListener(LookAtCamera);
    }
    
    private void OnDestroy() => _cameraRotationController.CameraChangedPosition.RemoveListener(LookAtCamera);
}

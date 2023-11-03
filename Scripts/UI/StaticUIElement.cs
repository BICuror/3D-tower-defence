using UnityEngine;

public sealed class StaticUIElement : IngameUIElement
{
    private void Start()
    {
        CameraRotationController.Instance.CameraRotationUpdated.AddListener(LookAtCamera);
    }
    
    private void OnDestroy() => CameraRotationController.Instance.CameraRotationUpdated.RemoveListener(LookAtCamera);
}

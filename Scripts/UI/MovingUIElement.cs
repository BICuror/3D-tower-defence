public sealed class MovingUIElement : IngameUIElement
{
    private void Update() 
    {
        LookAtCamera(CameraRotationController.Instance.Position);
    }
}

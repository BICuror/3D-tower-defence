using UnityEngine;

public class IngameUIElement : MonoBehaviour
{
    protected CameraRotationController _cameraRotationController;

    private void Awake() 
    {
        _cameraRotationController = FindObjectOfType<CameraRotationController>();
    }

    private void OnEnable()
    {   
        LookAtCamera();
    }

    protected void LookAtCamera()
    {
        transform.LookAt(_cameraRotationController.transform);

        transform.Rotate(90f, 0f, 0f);
    }
}

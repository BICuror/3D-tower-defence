using UnityEngine;

public class IngameUIElement : MonoBehaviour
{
    protected void LookAtCamera(Vector3 cameraPosition)
    {
        transform.LookAt(cameraPosition);

        transform.Rotate(90f, 0f, 0f);
    }
}

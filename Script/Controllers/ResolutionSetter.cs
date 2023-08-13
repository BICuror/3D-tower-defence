using UnityEngine;

public class ResolutionSetter : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(640, 480, true);
    }
}

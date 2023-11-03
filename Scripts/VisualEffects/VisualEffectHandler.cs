using UnityEngine;
using UnityEngine.VFX;

public sealed class VisualEffectHandler : MonoBehaviour
{
    [SerializeField] private StopActionType _stopAction;

    [SerializeField] private VisualEffect _visualEffect;

    private float _disableTime;

    private void Awake()
    {
        _disableTime = _visualEffect.GetFloat("MaxLifeTime"); 
    }

    public void Play()
    {
        _visualEffect.gameObject.SetActive(true);

        switch(_stopAction)
        {
            case StopActionType.Disable: Invoke("DisableThisObject", _disableTime); break;
            case StopActionType.Destroy: Invoke("DestroyThisObject", _disableTime); break;
        }
              
    }

    private void DisableThisObject()
    {
        _visualEffect.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    private void DestroyThisObject()
    {
        Destroy(gameObject);
    }

    private enum StopActionType
    {
        Disable, 
        Destroy
    }
}

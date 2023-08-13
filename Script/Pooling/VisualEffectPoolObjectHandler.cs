using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectPoolObjectHandler : MonoBehaviour
{
    [SerializeField] private StopActionType _stopAction;

    [SerializeField] private VisualEffect _visualEffect;

    private YieldInstruction _rechargeInstruction;

    private void Awake()
    {
        float disableTime = _visualEffect.GetFloat("MaxLifeTime"); 

        _rechargeInstruction = new WaitForSeconds(disableTime);
    }

    public void Play()
    {
        _visualEffect.Play();
    }

    public void Stop()
    {
        _visualEffect.Stop();

        ExecuteStopAction();
    }

    private IEnumerator StartDisableingProcess()
    {
        yield return _rechargeInstruction;

        gameObject.SetActive(false);
    }

    private void ExecuteStopAction()
    {
        switch (_stopAction)
        {
            case StopActionType.Destroy: Destroy(gameObject, _visualEffect.GetFloat("MaxLifeTime")); break;
            case StopActionType.Disable: StartCoroutine(StartDisableingProcess()); break;
        }
    }

    private enum StopActionType
    {
        Destroy,
        Disable
    }
}

using UnityEngine.Events;
using UnityEngine;

public sealed class AnimationExtendedEvent : MonoBehaviour
{
    public UnityEvent EventInvoked;

    public void InvokeEvent()
    {
        EventInvoked.Invoke();
    }
}

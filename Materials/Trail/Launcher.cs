using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class Launcher : MonoBehaviour
{
    public UnityEvent Landed;

    private DraggableObject _draggablePrefab;
    public void SetDraggablePrefab(DraggableObject draggable) => _draggablePrefab = draggable;

    public void Land(Vector3 landPosition)
    {
        DraggableObject draggable = Instantiate(_draggablePrefab, landPosition, Quaternion.identity);

        draggable.Place();

        Landed.Invoke();
    }
}

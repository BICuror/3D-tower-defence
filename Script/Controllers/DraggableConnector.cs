using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

public sealed class DraggableConnector : MonoBehaviour
{
    [Header("JointSettings")]

    [SerializeField] private Joint _joint;

    [SerializeField] private Vector3 _distance;

    [Header("PlacementSettings")]

    public UnityEvent<GameObject> PlacedDraggable;

    [SerializeField] private int _placementFramesDuration;

    private Vector3 rotStep;

    private record PlacementAnitationData
    {
        public readonly GameObject Object;

        public readonly Vector3 InitialPosition;
        public readonly Vector3 FinalPosition;

        public readonly Vector3 FinalRotation;
        public readonly Vector3 RotationSteps;

        public PlacementAnitationData(GameObject obj, Vector3 initPosition, Vector3 finalPosition, Vector3 finalRotation, Vector3 rotationSteps)
        {
            Object = obj;
            InitialPosition = initPosition;
            FinalPosition = finalPosition;
            FinalRotation = finalRotation;
            RotationSteps = rotationSteps;
        }
    }

    public void StartPlacementAnimation(GameObject objectToPlace, Vector3 finalPosition)
    {
        Vector3 rotationStep = new Vector3(GetRotationStep(objectToPlace.transform.rotation.eulerAngles.x), GetRotationStep(objectToPlace.transform.rotation.eulerAngles.x), GetRotationStep(objectToPlace.transform.rotation.eulerAngles.z));

        Debug.Log(objectToPlace.transform.rotation.eulerAngles.x.ToString() + "LOL");

        PlacementAnitationData currentAnimationData = new PlacementAnitationData(objectToPlace, objectToPlace.transform.position, finalPosition, Vector3.zero, rotationStep);

        StartCoroutine(PlaceObject(currentAnimationData));
    }

    private float GetRotationStep(float value)
    {
        value = Mathf.Abs(value);

        if (value > 180f) return (360f - value) / _placementFramesDuration; 
        else return -value / _placementFramesDuration;
    }

    private IEnumerator PlaceObject(PlacementAnitationData currentAnimationData)
    {
        YieldInstruction instruction = new WaitForFixedUpdate();

        for(float i = 0; i < _placementFramesDuration; i++)
        {
            yield return instruction;

            transform.position = Vector3.Lerp(transform.position, currentAnimationData.FinalPosition, i / (float)_placementFramesDuration);

            currentAnimationData.Object.transform.position = Vector3.Lerp(currentAnimationData.InitialPosition, currentAnimationData.FinalPosition, i / (float)_placementFramesDuration);

            currentAnimationData.Object.transform.Rotate(currentAnimationData.RotationSteps);
        }
        
        currentAnimationData.Object.transform.position = currentAnimationData.FinalPosition;

        currentAnimationData.Object.transform.rotation = Quaternion.Euler(0, currentAnimationData.FinalRotation.y, 0);

        PlaceDraggable(currentAnimationData.Object);
    }

    private void PlaceDraggable(GameObject draggable)
    {
        draggable.GetComponent<IDraggable>().Place();

        PlacedDraggable.Invoke(draggable);
    }

    public void ConnectDraggable(GameObject draggable)
    {
        draggable.transform.position = _joint.transform.position - _distance;

        Rigidbody rigidbody = draggable.GetComponent<Rigidbody>();

        rigidbody.useGravity = true;

        rigidbody.constraints = RigidbodyConstraints.None; 

        _joint.connectedBody = rigidbody;
    } 

    public void DisconnectDraggable(GameObject draggable)
    {
        Rigidbody rigidbody = draggable.GetComponent<Rigidbody>();

        rigidbody.useGravity = false;

        rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;;

        _joint.connectedBody = null;
    } 
}

using UnityEngine;
using UnityEngine.Events;

public sealed class ResourceGatherer : MonoBehaviour
{
    [SerializeField] private float _damageAmount;

    [SerializeField] private ResourceSourceAreaDetector _resourceSourceAreaDetector;

    [SerializeField] private MeshRenderer _mesh;

    private void Awake() 
    {
        GetComponent<TaskCycle>().ShouldWorkDelegate = ReturnTrue;
    
        GetComponent<DraggableObject>().Placed.AddListener(RotateTowardsSource);
    
        _resourceSourceAreaDetector.RemovedComponent.AddListener(RotateTowardsSource); 
    }

    private bool ReturnTrue() => _resourceSourceAreaDetector.IsEmpty() == false;

    private void RotateTowardsSource(ResourceSource source) => RotateTowardsSource();

    private void RotateTowardsSource()
    {
        if (_resourceSourceAreaDetector.IsEmpty() == false)
        {
            Vector3 sourcePosition = _resourceSourceAreaDetector.GetFirstEntityHealth().transform.position;
    
            _mesh.transform.LookAt(sourcePosition);

            _mesh.transform.Rotate(-32f, 180f, 0f);
        }
    }

    private void GatherResources()
    {
        EntityHealth currentSource = _resourceSourceAreaDetector.GetFirstEntityHealth();

        currentSource.GetHurt(_damageAmount);
    }
}

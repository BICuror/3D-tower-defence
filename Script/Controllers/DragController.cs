using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]

public sealed class DragController : MonoBehaviour
{
    [Header("RaycastSettings")]
    [SerializeField] private float _maxRaycastLength = 10000f;

    [Header("LayerSettings")]
    [SerializeField] private LayerSetting _draggableObjectLayerSettings;
    [SerializeField] private LayerSetting _solidObjectsLayerSettings;
    [SerializeField] private LayerSetting _terrainLayerSettings;
   
    [Header("DragSettings")]
    [SerializeField] private float _placingHeight;
    [SerializeField] private float _dragSpeed;

    [Header("Links")]
    [SerializeField] private DraggableConnector _draggableConnector;

    private Camera _camera;

    public UnityEvent<GameObject> PickedObject;
    public UnityEvent<GameObject> DroppedObject;

    private GameObject _currentDraggableGameObject;
    private IDraggable _currentIDraggable;
    private Vector3 _lastValuablePosition;

    private void OnEnable() => _camera = GetComponent<Camera>();

    public void DropDraggable(Vector2 mousePosition)
    {
        TryDragTo(mousePosition);

        _draggableConnector.DisconnectDraggable(_currentDraggableGameObject.gameObject);

        _draggableConnector.StartPlacementAnimation(_currentDraggableGameObject, GetLastSnappedGridPosition());

        DroppedObject.Invoke(_currentDraggableGameObject);

        _currentIDraggable = null;

        _currentDraggableGameObject = null;
    }

    private Vector3 GetLastSnappedGridPosition()
    {
        Vector3 placePosition = new Vector3(Mathf.RoundToInt(_lastValuablePosition.x), 0, Mathf.RoundToInt(_lastValuablePosition.z)); 

        Ray heightRay = new Ray(new Vector3(placePosition.x, _maxRaycastLength, placePosition.z), Vector3.down);

        if (Physics.Raycast(heightRay, out RaycastHit heightRayInfo, Mathf.Infinity, _terrainLayerSettings.GetLayerMask()))
        {
            return new Vector3(placePosition.x, heightRayInfo.point.y + _placingHeight, placePosition.z);
        }

        return Vector3.zero;
    }

    public void PickUpDraggable(Vector2 mousePosition)
    {
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit rayInfo, Mathf.Infinity, _draggableObjectLayerSettings.GetLayerMask()))
        {
            _currentDraggableGameObject = rayInfo.collider.gameObject;
            _currentIDraggable = rayInfo.collider.gameObject.GetComponent<IDraggable>();
            _lastValuablePosition = rayInfo.collider.transform.position;
            _draggableConnector.transform.position = rayInfo.collider.transform.position;

            _currentIDraggable.PickUp();

            _draggableConnector.ConnectDraggable(_currentDraggableGameObject);

            PickedObject.Invoke(_currentDraggableGameObject);
        }
    }

    public void TryDragTo(Vector2 mousePosition)
    {
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit rayInfo, Mathf.Infinity, _terrainLayerSettings.GetLayerMask()))
        {
            Vector3 roundedRayPosition = new Vector3(Mathf.RoundToInt(rayInfo.point.x), Mathf.RoundToInt(rayInfo.point.y), Mathf.RoundToInt(rayInfo.point.z));

            Ray heightRay = new Ray(new Vector3(roundedRayPosition.x, _maxRaycastLength, roundedRayPosition.z), Vector3.down);

            if (Physics.Raycast(heightRay, out RaycastHit heightRayInfo, Mathf.Infinity, _terrainLayerSettings.GetLayerMask()))
            {
                if (HasBuildingAt(roundedRayPosition.x, roundedRayPosition.z) == false)
                {
                    _lastValuablePosition = new Vector3(roundedRayPosition.x, heightRayInfo.point.y + _placingHeight, roundedRayPosition.z);
                }
            }
        }

        MoveDraggable();
    }

    private void MoveDraggable()
    {
        float distance = Vector3.Distance(_lastValuablePosition, _draggableConnector.transform.position);

        _draggableConnector.transform.position = Vector3.MoveTowards(_draggableConnector.transform.position, _lastValuablePosition, _dragSpeed * distance);        
    }

    public bool HasBuildingAt(float x, float z)
    {
        RaycastHit[] hits = Physics.RaycastAll(new Vector3(x, _maxRaycastLength, z), Vector3.down, Mathf.Infinity, _solidObjectsLayerSettings.GetLayerMask());

        if (hits.Length == 0) return false;
        else if (hits.Length == 1 && hits[0].collider.gameObject == _currentDraggableGameObject) return true;
        else return true;
    }

    public bool PickedUpDraggable(Vector2 mousePosition)
    {
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit rayInfo, _maxRaycastLength, _draggableObjectLayerSettings.GetLayerMask()))
        {
            if (rayInfo.collider.gameObject.TryGetComponent(out IDraggable draggable))
            {
                return draggable.IsDraggable();
            }
        }

        return false;
    }

    public bool ActivatedSomething(Vector2 mousePosition)
    {
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit rayInfo, _maxRaycastLength, _draggableObjectLayerSettings.GetLayerMask()))
        {
            if (rayInfo.collider.gameObject.TryGetComponent(out IActivatable activatable))
            {
                activatable.Activate();

                return true;
            }
        }

        return false;
    }
}

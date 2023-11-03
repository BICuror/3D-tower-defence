using UnityEngine;
using UnityEngine.Events;
    
[RequireComponent(typeof(Controller))]

public sealed class CameraRotationController : MonoBehaviour
{       
    #region Singletone
    private static CameraRotationController _instance;
    public static CameraRotationController Instance => _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else 
        {
            Destroy(gameObject);

            Debug.LogError("Multiple rotation controller instance");
        }
    }   
    #endregion

    [SerializeField] private float _distanceToTarget;
    [Range(0f, 85f)] [SerializeField] private float _maxYRotation;
    [Range(0f, 85f)] [SerializeField] private float _minYRotation;

    [Range(1f, 1000f)] [SerializeField] private float _sensetivity;

    private Camera _camera;
    private Transform _target;

    private Vector3 _previousPosition;

    public UnityEvent<Vector3> CameraRotationUpdated;
    public Vector3 Position => transform.position;

    private void OnEnable() => _camera = GetComponent<Camera>();
    
    public void SetTarget(Transform newTarget) 
    {
        _target = newTarget; 
    
        Rotate(Vector2.zero);
    }

    public void SetPreviousMousePosition(Vector2 mousePosition) => _previousPosition = _camera.ScreenToViewportPoint(mousePosition);

    public void Rotate(Vector2 touchPosition)
    {       
        Vector3 newPosition = _camera.ScreenToViewportPoint(touchPosition);
        Vector3 direction = _previousPosition - newPosition;
        
        float rotationAroundYAxis = -direction.x * _sensetivity; // camera moves horizontally
        float rotationAroundXAxis = direction.y * _sensetivity; // camera moves vertically

        float currentRotation = transform.rotation.eulerAngles.x;

        transform.position = _target.position;
            
        if (rotationAroundXAxis + currentRotation >= _maxYRotation) rotationAroundXAxis = _maxYRotation - currentRotation;
        else if (rotationAroundXAxis + currentRotation <= _minYRotation) rotationAroundXAxis = _minYRotation - currentRotation;  
        
        transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
        
        transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); 

        transform.Translate(new Vector3(0, 0, -_distanceToTarget));

        _previousPosition = newPosition;

        UpdateCamera();
    }

    public void UpdateCamera()
    {
        CameraRotationUpdated.Invoke(Position);
    }
}

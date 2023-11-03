using UnityEngine;

[RequireComponent(typeof(Camera))]

public sealed class CameraZoomController : MonoBehaviour
{
    [SerializeField] private float _zoomSensetivity;
    [SerializeField] private float _minZoomValue;
    [SerializeField] private float _maxZoomValue;
    private Camera _camera;
    
    private float _startZoom;
    private float _previousZoom;

    private void OnEnable() => _camera = GetComponent<Camera>();

    public void StartZooming(Vector2 firstTouchPosition, Vector2 secondTouchPosition)
    {
        _previousZoom = Vector2.Distance(firstTouchPosition, secondTouchPosition);
    }
    
    public void Zooming(Vector2 firstTouchPosition, Vector2 secondTouchPosition)
    {
        float zoomValue = (_previousZoom - Vector2.Distance(firstTouchPosition, secondTouchPosition)) * _zoomSensetivity;

        _previousZoom = zoomValue;

        if (_camera.orthographicSize + zoomValue > _minZoomValue && _camera.orthographicSize + zoomValue < _maxZoomValue)
        {
            _camera.orthographicSize = _camera.orthographicSize + zoomValue;
        }
    }
}

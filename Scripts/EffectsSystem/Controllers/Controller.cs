using UnityEngine;
using TMPro;

[RequireComponent(typeof(Camera))]

public sealed class Controller : MonoBehaviour
{
    private Controls _controls;

    [SerializeField] private DragController _dragController;
    [SerializeField] private CameraRotationController _cameraRotationController;
    [SerializeField] private CameraZoomController _cameraZoomController;

    [SerializeField] private TextMeshProUGUI _ugui;
    [SerializeField] private TextMeshProUGUI _state;

    public ControllerState _currentControllerState;

    public enum ControllerState 
    {
        Idle,
        Dragging,
        Rotating,
        Zooming
    }   

    private void Setup()
    {
        _controls.TouchInput.FirstTouchEvent.started += _ => TryPickUpDraggableOrRotateCamera();

        _controls.TouchInput.FirstTouchEvent.canceled += _ => ReturnToIdleState();

        _controls.TouchInput.SecondTouchEvent.started += _ => StartZooming();

        _controls.TouchInput.SecondTouchEvent.canceled += _ => StopZooming();
    }

    private void FixedUpdate()
    {
        _state.text = _currentControllerState.ToString();

        switch(_currentControllerState)
        {
            case ControllerState.Idle: return;
            case ControllerState.Dragging: _dragController.TryDragTo(GetFirstTouchPosition()); break;
            case ControllerState.Rotating: _cameraRotationController.Rotate(GetFirstTouchPosition()); break;
            case ControllerState.Zooming: _cameraZoomController.Zooming(GetFirstTouchPosition(), GetSecondTouchPosition()); break;
        }
    }

    private void TryPickUpDraggableOrRotateCamera()
    {
        if (_dragController.PickedUpDraggable(GetFirstTouchPosition()))
        {
            _currentControllerState = ControllerState.Dragging;

            _dragController.PickUpDraggable(GetFirstTouchPosition());
        }
        else if (_dragController.ActivatedSomething(GetFirstTouchPosition()) == false)
        {
            _cameraRotationController.SetPreviousMousePosition(GetFirstTouchPosition());
            
            _currentControllerState = ControllerState.Rotating;   
        }
    }

    private void ReturnToIdleState()
    {
        if (_currentControllerState == ControllerState.Dragging)
        {
            _dragController.DropDraggable(GetFirstTouchPosition());
        }

        _currentControllerState = ControllerState.Idle;
    }

    private void StartZooming()
    {
        if (_currentControllerState != ControllerState.Dragging)
        {
            _cameraZoomController.StartZooming(GetFirstTouchPosition(), GetSecondTouchPosition());
            
            _currentControllerState = ControllerState.Zooming;
        }
    }

    private void StopZooming()
    {
        _currentControllerState = ControllerState.Rotating;
    }

    private Vector2 GetFirstTouchPosition()
    {
        _ugui.text = (_controls.TouchInput.FirstTouch.ReadValue<Vector2>().x.ToString() + " " + _controls.TouchInput.FirstTouch.ReadValue<Vector2>().y.ToString());

        return _controls.TouchInput.FirstTouch.ReadValue<Vector2>();
    }
    private Vector2 GetSecondTouchPosition()
    {
        return _controls.TouchInput.SecondTouch.ReadValue<Vector2>();
    }

    #region Enable\Disable region
    
    private void OnEnable() 
    {
        _controls = new Controls();

        _controls.Enable();

        Setup();
    } 

    private void OnDisable() 
    {
        _controls.Disable();

        _controls = null;
    }

    #endregion 
}
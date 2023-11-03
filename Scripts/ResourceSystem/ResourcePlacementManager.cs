using UnityEngine;

public sealed class ResourcePlacementManager : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private GameObject _itemMesh;

    [Header("RlacementSettings")]
    [SerializeField] private float _maxAdditionalYRotation;    
    [SerializeField] private float _maxPositionFromCenter;

    private Vector3 _startLocalPosition;

    private void Awake() 
    {
        _startLocalPosition = _itemMesh.transform.localPosition;

        RandomizePlacement();
    }
    
    public void RandomizePlacement()
    {
        _itemMesh.transform.Rotate(0f, Random.Range(-_maxAdditionalYRotation, _maxAdditionalYRotation), 0f);

        _itemMesh.transform.localPosition = new Vector3(Random.Range(-_maxPositionFromCenter, _maxPositionFromCenter), _startLocalPosition.y, Random.Range(-_maxPositionFromCenter, _maxPositionFromCenter));
    }

    public void PlaceNormally()
    {
        _itemMesh.transform.localPosition = _startLocalPosition;
    }
}

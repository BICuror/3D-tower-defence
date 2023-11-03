using UnityEngine;

public sealed class AreaManager : MonoBehaviour
{
    [Header("AreaSettings")]
    [SerializeField] private float _height = 100f;
    [SerializeField] private int _radius;
    [SerializeField] private GameObject _reachAreaCollider;

    private void Awake() => UpdateScale();

    public void SetRaduis(int value)
    {
        _radius = value;

        UpdateScale();
    }

    public void UpdateScale()
    {
        _reachAreaCollider.transform.localScale = GetScale();
    }

    public Vector3 GetScale()
    {
        float scale = _radius * 2f + 0.95f;

        return new Vector3(scale, _height, scale);
    }
}

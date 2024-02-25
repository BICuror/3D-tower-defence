using UnityEngine;

public sealed class TimeChanger : MonoBehaviour, IActivatable
{
    [Header("DisplaySettings")]
    [SerializeField] private SpriteRenderer _image;

    [Header("TimeSettings")]
    [SerializeField] private float[] _timeScales;
    [SerializeField] private Sprite[] _sprites;

    private int _lastIndex;

    public void Activate()
    {
        int index = _lastIndex+1;

        if (index >= _timeScales.Length) index = 0;

        _lastIndex = index;

        Time.timeScale = _timeScales[_lastIndex];

        _image.sprite = _sprites[_lastIndex];
    }
}


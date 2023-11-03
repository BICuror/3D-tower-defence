using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    private float _decreaseTime = 0.2f;

    private float _timeBeforeDecrease = 0.8f;

    private float _shakeDuration = 0.27f;

    private float _shakeStrength = 6f;

    private MeshRenderer _meshRenderer;

    private float _lastSetHealthDifferecne;

    private YieldInstruction _yieldInstruction = new WaitForFixedUpdate();

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        _meshRenderer.SetPropertyBlock(new MaterialPropertyBlock());
    }

    public void SetValue(float health) => SetPropertyBlock(health, health);

    public void DecreaseValue(float currentHealth, float healthDifference)
    {
        if (_lastSetHealthDifferecne < healthDifference) _lastSetHealthDifferecne = healthDifference;
        else _lastSetHealthDifferecne *= 0.95f;

        SetPropertyBlock(currentHealth, _lastSetHealthDifferecne);

        StopAllCoroutines();

        StartCoroutine(StartDecreasingHealthDifference(currentHealth));
    }

    public void IncreaseValue(float currentHealth)
    {
        float healthDifference = currentHealth;

        StopAllCoroutines();

        ShakeSlider();

        if (currentHealth < _lastSetHealthDifferecne) healthDifference = _lastSetHealthDifferecne;
            
        SetPropertyBlock(currentHealth, healthDifference);
    }

    private void SetPropertyBlock(float currentHealth, float healthDifference)
    {
        MaterialPropertyBlock healthDifferenceBlock = new MaterialPropertyBlock();
        healthDifferenceBlock.SetFloat("Health", currentHealth);
        healthDifferenceBlock.SetFloat("HealthDifference", healthDifference);
        _meshRenderer.SetPropertyBlock(healthDifferenceBlock);
    }

    private IEnumerator StartDecreasingHealthDifference(float currentHealth)
    {
        StartCoroutine(ShakeSlider());

        yield return new WaitForSeconds(_timeBeforeDecrease);

        StartCoroutine(DecreaseSlider(currentHealth));
    }

    private IEnumerator ShakeSlider()
    {
        SetToDefaultPosition();

        float amountOfSetps = Mathf.RoundToInt(50f * _shakeDuration);

        float rotationValue = Random.Range(0.7f, 1.5f);
        if (Random.Range(0, 100) > 50) rotationValue *= -1;

        float positionValue = Random.Range(0.7f, 1.5f);
        if (Random.Range(0, 100) > 50) positionValue *= -1;

        for(int i = 0; i <= amountOfSetps; i++)
        {
            float evaluatedValue = i / amountOfSetps;

            transform.Rotate(0f, rotationValue * Mathf.Sin(Mathf.Deg2Rad * evaluatedValue * 720f) * _shakeStrength, 0f);

            transform.localPosition = new Vector3(0f, 0f, positionValue * Mathf.Sin(Mathf.Deg2Rad * evaluatedValue * 360) * _shakeStrength * 0.02f);
            
            yield return _yieldInstruction;
        }
    }

    private IEnumerator DecreaseSlider(float currentHealth)
    {
        float amountOfSetps = Mathf.RoundToInt(50f * _decreaseTime);

        float healthDecreaseStep = (_lastSetHealthDifferecne - currentHealth) / amountOfSetps;

        for(int i = 0; i < amountOfSetps; i++)
        {
            _lastSetHealthDifferecne -= healthDecreaseStep;

            SetPropertyBlock(currentHealth, _lastSetHealthDifferecne);
            
            yield return _yieldInstruction;
        }
    }

    private void SetToDefaultPosition()
    {
        transform.localRotation = Quaternion.identity;

        transform.localPosition = Vector3.zero;
    }
}

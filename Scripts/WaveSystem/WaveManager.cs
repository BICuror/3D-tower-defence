using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public sealed class WaveManager : MonoBehaviour
{
    [SerializeField] private float _waveEndingDuration;
    [SerializeField] private float _wavePreparationDuration;

    private int _currentWave;
    public UnityEvent<float> WaveStopped;
    public UnityEvent<float> WavePreparationBegun;
    public UnityEvent WaveStarted;

    public int GetCurrentWave() => _currentWave;
    
    public void StopWave()
    {
        WaveStopped.Invoke(_waveEndingDuration);

        StartCoroutine(WaitToStopWave());
    }
    
    private IEnumerator WaitToStopWave()
    {
        yield return new WaitForSeconds(_waveEndingDuration);

        PrepeareWave();
    }   

    public void PrepeareWave()
    {
        WavePreparationBegun.Invoke(_wavePreparationDuration);

        StartCoroutine(WaitToPrepeareWave());
    }

    private IEnumerator WaitToPrepeareWave()
    {
        yield return new WaitForSeconds(_wavePreparationDuration);

        StartWave();
    }   

    private void StartWave()
    {
        _currentWave++;

        WaveStarted.Invoke();
    }
}

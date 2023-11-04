using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Zenject;

public sealed class WaveManager : MonoBehaviour
{
    [Inject] private EnemySpawnerSystem _enemySpawnerSystem;
    [Inject] private EnemyBiomeContainer _enemyBiomesContainer;
    [Inject] private RoadGenerator _roadGenerator;
    [Inject] private EnemyBiomeGenerator _enemyBiomeGenerator;

    [SerializeField] private float _waveEndingDuration;
    [SerializeField] private float _wavePreparationDuration;

    private int _currentWave;
    
    public UnityEvent<float> WaveStopped;
    public UnityEvent<float> WavePreparationBegun;

    public UnityEvent WaveStarted;

    private bool _waveIsReadyToBeStarted;

    public int GetCurrentWave() => _currentWave;
    
    public void StopWave()
    {
        WaveStopped.Invoke(_waveEndingDuration);

        _enemyBiomesContainer.DisableBiomesTerrain(_waveEndingDuration);
        _enemyBiomesContainer.IncreaseBiomesStages();

        StartCoroutine(WaitToStopWave());
    }
    
    private IEnumerator WaitToStopWave()
    {
        yield return new WaitForSeconds(_waveEndingDuration);

        PrepeareWave();
    }   

    public void PrepeareWave()
    {     
        _enemyBiomesContainer.DestroyOldBiomes();
        _enemyBiomeGenerator.TryGenerateNewBiome();
        _roadGenerator.GenerateRoads();
        _enemyBiomesContainer.RegenerateBiomes();
        _enemyBiomesContainer.GenerateBiomesDecorations();
        _enemyBiomesContainer.EnableBiomesTerrain(_wavePreparationDuration);

        WavePreparationBegun.Invoke(_wavePreparationDuration);
        StartCoroutine(WaitToPrepeareWave());
    }

    private IEnumerator WaitToPrepeareWave()
    {
        yield return new WaitForSeconds(_wavePreparationDuration);
    
        TryToStartWave();
    }   

    public void TryToStartWave()
    {
        if (_waveIsReadyToBeStarted == false)
        {
            _waveIsReadyToBeStarted = true;
        }
        else
        {
            _waveIsReadyToBeStarted = false;

            StartWave();
        }
    }

    private void StartWave()
    {
        _currentWave++;

        WaveStarted.Invoke();

        _enemySpawnerSystem.StartWave();
    }
}

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Zenject;

public sealed class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    [Inject] private EnemySpawnerSystem _enemySpawnerSystem;

    [Inject] private EnemyBiomeContainer _enemyBiomesContainer;

    [Inject] private RoadGenerator _roadGenerator;

    [Inject] private EnemyBiomeGenerator _enemyBiomeGenerator;

    [SerializeField] private float _waveEndingDuration;
    [SerializeField] private float _wavePreparationDuration;

    [SerializeField] private TerrainAnimator _roadTerrainAnimator;

    private int _currentWave;
    public UnityEvent<float> WaveStopped;

    public UnityEvent<float> WavePreparationBegun;
    public UnityEvent WaveStarted;

    public UnityEvent<float> FirstWaveStarted;

    private bool _waveIsReadyToBeStarted;

    public int GetCurrentWave() => _currentWave;
    
    public void StopWave()
    {
        WaveStopped.Invoke(_waveEndingDuration);

        _roadTerrainAnimator.StartDisappearing(_waveEndingDuration);

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
        Instance = this;        

        _enemyBiomesContainer.DestroyOldBiomes();

        _enemyBiomeGenerator.TryGenerateNewBiome();

        _roadGenerator.GenerateRoads();

        _enemyBiomesContainer.RegenerateBiomes();

        _roadTerrainAnimator.StartAppearing(_wavePreparationDuration);

        _enemyBiomesContainer.GenerateBiomesDecorations();

        _enemyBiomesContainer.EnableBiomesTerrain(_wavePreparationDuration);
        WavePreparationBegun.Invoke(_wavePreparationDuration);
        StartCoroutine(WaitToPrepeareWave());
    }

    private IEnumerator WaitToPrepeareWave()
    {
        Debug.Log("Starting wave...");

        yield return new WaitForSeconds(_wavePreparationDuration);
        TryToStartWave();
    }   

    public void TryToStartWave()
    {
        Debug.Log("TriedToStartAWave");
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

    public void PrepeareFirstWave()
    {
        FirstWaveStarted.Invoke(_wavePreparationDuration);

        
    }
}

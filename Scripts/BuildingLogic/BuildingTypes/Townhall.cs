using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Zenject;

public sealed class Townhall : MonoBehaviour
{
    [Inject] private IslandData _islandData;
    [Inject] private WaveManager _waveManager;
    [Inject] private DraggableCreator _draggableCreator;

    [Header("Links")]
    [SerializeField] private TownhallCrystalDetector _townhallCrystalDetector;
    [SerializeField] private Transform _creationSource;

    [Header("CrystalCreation")] 
    [SerializeField] private DraggableObject _crystalPrefab;
    [SerializeField] private Launcher _crystalLauncher;

    [Header("TowerCreation")]
    [SerializeField] private DraggableObject _towerToCreate;
    [SerializeField] private LayerSetting _terrainSetting;
    [SerializeField] private int _towerAmount;
    [SerializeField] private int _towerCreationRadius;

    public UnityEvent CrystalPlaced;
    public UnityEvent CrystalDestroyed;

    private void Awake()
    {
        GetComponent<IDraggable>().Place();

        _townhallCrystalDetector.PlacedComponentAdded.AddListener(DestroyCrystal);

        _waveManager.WavePreparationBegun.AddListener(CreateCrystal);
        //_waveManager.WavePreparationBegun.AddListener(CreateTowers);
    }
    
    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition + Vector3.up;
        FindObjectOfType<CameraRotationController>().SetTarget(transform);
    }

    private void CreateCrystal(float blank)
    {
        _draggableCreator.CreateDraggableOnRandomPosition(_crystalPrefab, _creationSource.position, 3, null, _crystalLauncher);

        CrystalDestroyed.Invoke();
    }

    private void DestroyCrystal(TownhallCrystal townhallCrystal)
    {
        Destroy(townhallCrystal.gameObject);

        CrystalPlaced.Invoke();
    }

    private void OnDestroy()
    {
        SceneManager.LoadScene(0);
    }

    public void CreateTowers(float blank)
    {
        for (int i = 0; i < _towerAmount; i++)
        {
            _draggableCreator.CreateDraggableOnRandomPosition(_towerToCreate, _creationSource.position, _towerCreationRadius, _terrainSetting);
        }
    }
}


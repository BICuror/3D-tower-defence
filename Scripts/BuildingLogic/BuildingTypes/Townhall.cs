using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Zenject;

public sealed class Townhall : MonoBehaviour
{
    [SerializeField] private TownhallCrystalDetector _townhallCrystalDetector;
 
    [Inject] private IslandData _islandData;

    [Inject] private WaveManager _waveManager;

    [Inject] private HeightMapGenerator _gen;

    [Inject] private RoadMapGenerator _geeeen;

    [SerializeField] private DraggableObject _crystalPrefab;

    [SerializeField] private DraggableObject _chest;

    [SerializeField] private Transform _creationSource;

    [SerializeField] private LayerSetting _terrainSetting;

    [SerializeField] private Launcher _crystalLauncher;

    [SerializeField] private int amoun;

    [SerializeField] private int _radius;

    public UnityEvent CrystalPlaced;

    public UnityEvent CrystalDestroyed;

    private void Awake()
    {
        GetComponent<IDraggable>().Place();

        _townhallCrystalDetector.PlacedComponentAdded.AddListener(DestroyCrystal);

        _waveManager.WavePreparationBegun.AddListener(CreateCrystal);
        _waveManager.WavePreparationBegun.AddListener(Crts);
        _waveManager.FirstWaveStarted.AddListener(CreateCrystal);
    }
    
    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition + Vector3.up;
        FindObjectOfType<CameraRotationController>().SetTarget(transform);
    }

    private void CreateCrystal(float blank)
    {
        DraggableCreator.Instance.CreateDraggableOnRandomPosition(_crystalPrefab, _creationSource.position, 3, null, _crystalLauncher);

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

    public void Crts(float t)
    {
        int center = (int)(_islandData.IslandSize / 2) + 1;

        List<Vector3> pos = new List<Vector3>();

        bool[,] roadMap = _geeeen.RoadMap;

        int[,] heightMap = _gen.HeightMap;

        for (int x = -_radius; x <= _radius; x++)
        {
            for (int z = -_radius; z <= _radius; z++)
            {
                int currentX = center + x;
                int currentZ = center + z;

                if (roadMap[currentX, currentZ])// && heightMap[currentX, currentZ] > 0)
                {
                    float height = heightMap[currentX, currentZ];

                    if (height <= 0) height++;

                    pos.Add(new Vector3(currentX, height + 1f, currentZ));
                }
            }
        }

        for (int i = 0; i < amoun; i++)
        {
            int index = Random.Range(0, pos.Count);

            DraggableCreator.Instance.CreateDraggableOnRandomPosition(_chest, _creationSource.position, _radius, _terrainSetting);

            pos.RemoveAt(index);
        }
    }
}


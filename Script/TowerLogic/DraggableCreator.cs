using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public sealed class DraggableCreator : MonoBehaviour 
{
    private static DraggableCreator _instance;
    public static DraggableCreator Instance => _instance;

    [Header("SpawnPositionSettings")]
    [SerializeField] private GameObject _blankDraggable;

    [SerializeField] private LayerSetting _terrainLayerSettings;    
    [SerializeField] private LayerSetting _nonSolidObjectsLayerSettings;   

    [Range(1f, 5f)] [SerializeField] private int _spawnRadius;

    [Space] [Header("LaunchSettings")]
    [SerializeField] private float _launchDuration;

    [SerializeField] private float _launchMaxHeight;
    
    [SerializeField] private Launcher _launcherPrefab;
    
    private YieldInstruction _yieldInstruction = new WaitForFixedUpdate(); 

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else 
        {
            Debug.LogError($"Found other instance of {this.GetType().ToString()}");

            Destroy(gameObject);
        }
    }

    public void CreateDraggable(DraggableObject draggablePrefab, Vector3 centerPosition, Vector3 finalPosition)
    {       
        Launcher launcher = CreateLauncher(centerPosition, finalPosition);

        launcher.SetDraggablePrefab(draggablePrefab);

        CreateBlankDraggable(finalPosition);
    }

    public void CreateDraggable(DraggableObject draggablePrefab, Vector3 centerPosition)
    {
        Vector3 finalPosition = GetRandomSpawnPosition(centerPosition);
            
        Launcher launcher = CreateLauncher(centerPosition, finalPosition);

        launcher.SetDraggablePrefab(draggablePrefab);

        CreateBlankDraggable(finalPosition);
    }

    private void CreateBlankDraggable(Vector3 spawnPosition)
    {
        GameObject blankDraggable = Instantiate(_blankDraggable, spawnPosition, Quaternion.identity);

        Destroy(blankDraggable, _launchDuration);
    }

    #region LauncherCreation
    private Launcher CreateLauncher(Vector3 centerPosition, Vector3 finalPosition)
    {
        Launcher launcher = Instantiate(_launcherPrefab, centerPosition, Quaternion.identity);

        StartCoroutine(LaunchLauncher(launcher, finalPosition));

        return launcher;
    }

    private IEnumerator LaunchLauncher(Launcher launcher, Vector3 finalPosition)
    {
        Vector3 startPosition = launcher.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < _launchDuration)
        {
            yield return _yieldInstruction;

            elapsedTime += Time.deltaTime;

            float currentProgress = elapsedTime / _launchDuration;

            Vector3 evaluetedPosition = Vector3.Lerp(startPosition, finalPosition, currentProgress);

            evaluetedPosition.y = evaluetedPosition.y + (Mathf.Sin(currentProgress * 180f * Mathf.Deg2Rad)) * _launchMaxHeight; 

            launcher.transform.position = evaluetedPosition;
        }

        launcher.Land(finalPosition);
    }
    #endregion

    #region SpawnPositionPicking
    private Vector3 GetRandomSpawnPosition(Vector3 centerPosition)
    {
        List<Vector3> allPossiblePositions = new List<Vector3>();

        for (int i = 1; i <= _spawnRadius; i++)
        {
            allPossiblePositions.AddRange(GetPossiblePositionsInRadius(centerPosition, i));
        }

        int currentRadius = _spawnRadius;

        while(allPossiblePositions.Count == 0)
        {
            currentRadius++;

            allPossiblePositions.AddRange(GetPossiblePositionsInRadius(centerPosition, currentRadius));
        }
        
        return allPossiblePositions[Random.Range(0, allPossiblePositions.Count)];
    }

    private List<Vector3> GetPossiblePositionsInRadius(Vector3 centerPosition, float radius)
    {
        List<Vector3> possiblePositions = new List<Vector3>();

        float xMinPosition = centerPosition.x - radius;
        float xMaxPosition = centerPosition.x + radius;

        float zMinPosition = centerPosition.z - radius;
        float zMaxPosition = centerPosition.z + radius;

        for (float x = xMinPosition; x <= xMaxPosition; x++)
        {
            for (float z = zMinPosition; z <= zMaxPosition; z++)
            {
                if (x == xMinPosition || z == zMinPosition || x == xMaxPosition || z == zMaxPosition )
                {
                    Ray heightRay = new Ray(new Vector3(x, 10000f, z), Vector3.down);
                    
                    if (Physics.Raycast(heightRay, out RaycastHit rayInfo, Mathf.Infinity, ~_nonSolidObjectsLayerSettings.GetLayerMask()))
                    {
                        if (((1 << rayInfo.collider.gameObject.layer) & _terrainLayerSettings.GetLayerMask()) != 0)
                        {
                            possiblePositions.Add(new Vector3(x, rayInfo.point.y + 0.5f, z));
                        }
                    }
                }
            }
        }
        return possiblePositions;
    }
    #endregion
}

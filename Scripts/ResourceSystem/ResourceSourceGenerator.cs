using UnityEngine;
using System.Collections.Generic;
using Zenject;

public sealed class ResourceSourceGenerator : MonoBehaviour
{       
    [Inject] IslandData _islandData;

    [SerializeField] private LayerMask _takenLayers;

    [SerializeField] private LayerMask _terrainLayer; 

    public void GenerateResources()
    {
        ResourceSourcesList.SetupList();        

        for (int i = 0; i < _islandData.Resources.Length; i++)
        {
            for (int y = 0; y < _islandData.Resources[i].SourcesAmount; y++)
            {
                TryGenerateResourceSource(_islandData.Resources[i].SourcePrefab);
            }
        }
    }

    private void TryGenerateResourceSource(ResourceSource prefabToGenerate)
    {
        while (true)
        {
            int x = Random.Range(0, _islandData.IslandSize);
            int z = Random.Range(0, _islandData.IslandSize);

            Ray heightRay = new Ray(new Vector3(x, _islandData.IslandMaxHeight, z), Vector3.down);

            if (Physics.Raycast(heightRay, Mathf.Infinity, _takenLayers) == false)
            {
                if (Physics.Raycast(heightRay, out RaycastHit heightRayInfo, Mathf.Infinity, _terrainLayer) == true)
                {
                    GenerateResourceSource(new Vector3(x, heightRayInfo.point.y + 0.5f, z), prefabToGenerate);

                    return;
                } 
            }
        }
    }

    private void GenerateResourceSource(Vector3 position, ResourceSource prefabToGenerate)
    {
        GameObject source = Instantiate(prefabToGenerate.gameObject, position, Quaternion.identity);   

        ResourceSourcesList.Add(source.GetComponent<ResourceSource>());

        source.GetComponent<ResourceSource>().SourceDestroyed.AddListener(ResourceSourcesList.Remove);

        source.GetComponent<ResourceSource>().SourceDestroyed.AddListener(TryGenerateResourceSource);
    }
}

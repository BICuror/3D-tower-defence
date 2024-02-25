using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace WorldGeneration
{
    public class NavMeshLinksGenerator : MonoBehaviour
    {
        [Inject] private IslandDataContainer _islandDataContainer;
        private IslandData _islandData => _islandDataContainer.Data;

        [SerializeField] private GameObject _jumpLink;

        private List<GameObject> _jumpLinks = new List<GameObject>();

        public void DestroyAllStairs()
        {
            for (int i = 0; i < _jumpLinks.Count; i++)
            {
                Destroy(_jumpLinks[i]);
            }

            _jumpLinks = new List<GameObject>();
        }

        public void GenrateStairs(int[,] heightMap, bool[,] roadMap)
        {
            _jumpLinks = new List<GameObject>();

            for (int x = 0; x < _islandData.IslandSize; x++)
            {
                for (int z = 0; z < _islandData.IslandSize; z++) 
                {
                    if (roadMap[x, z] == true)
                    {
                        int currentHeight = heightMap[x, z];

                        if (x + 1 < _islandData.IslandSize && roadMap[x + 1, z] && heightMap[x + 1, z] != currentHeight)
                        {
                            GenerateStair(new Vector3(x, currentHeight, z), new Vector3(x + 1, heightMap[x + 1, z], z));
                        } 
                        if (x - 1 >= 0 && roadMap[x - 1, z] && heightMap[x - 1, z] != currentHeight)
                        {
                            GenerateStair(new Vector3(x, currentHeight, z), new Vector3(x - 1, heightMap[x - 1, z], z));
                        } 
                        if (z + 1 < _islandData.IslandSize && roadMap[x, z + 1] && heightMap[x, z + 1] != currentHeight)
                        {
                            GenerateStair(new Vector3(x, currentHeight, z), new Vector3(x, heightMap[x, z + 1], z + 1));
                        } 
                        if (z - 1 >= 0 && roadMap[x, z - 1] && heightMap[x, z - 1] != currentHeight)
                        {
                            GenerateStair(new Vector3(x, currentHeight, z), new Vector3(x, heightMap[x, z - 1], z - 1));
                        } 
                    }
                }
            }
        }

        private void GenerateStair(Vector3 currentPos, Vector3 dir)
        {
            if (currentPos.y > dir.y) return; 

            _jumpLinks.Add(Instantiate(_jumpLink, Vector3.zero + new Vector3(0f, 0.5f, 0f), Quaternion.identity));

            NavMeshLink link = _jumpLinks[_jumpLinks.Count - 1].GetComponent<NavMeshLink>();

            link.startPoint = new Vector3(Mathf.Lerp(currentPos.x, dir.x, 0.1f), currentPos.y, Mathf.Lerp(currentPos.z, dir.z, 0.1f));
            link.endPoint = new Vector3(Mathf.Lerp(dir.x, currentPos.x, 0.1f), dir.y, Mathf.Lerp(dir.z, currentPos.z, 0.1f));
        }
    }
}
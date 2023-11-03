using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCreatorTower : MonoBehaviour
{
    [SerializeField] private Bomb _bombPrefab;

    private Bomb _currentBomb;

    private void Start()
    {
        GetComponent<DraggableObject>().Placed.AddListener(CreateBomb);        
    }

    private void CreateBomb()
    {
        DraggableCreator.Instance.CreateDraggableOnRandomPosition(_bombPrefab, transform.position);
    }
}

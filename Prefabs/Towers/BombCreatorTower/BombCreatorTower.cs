using UnityEngine;
using Zenject;

public sealed class BombCreatorTower : MonoBehaviour
{
    [Inject] private DraggableCreator _draggableCreator;

    [SerializeField] private Bomb _bombPrefab;

    private void CreateBomb()
    {
        _draggableCreator.CreateDraggableOnRandomPosition(_bombPrefab, transform.position);
    }
}

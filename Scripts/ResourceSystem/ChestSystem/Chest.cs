using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Chest : MonoBehaviour
{
    [SerializeField] private DraggableObject[] _draggable; 

    private void Awake() => WaveManager.Instance.WaveStopped.AddListener(CreateBliank);

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Enemy")) Destroy(gameObject);
    }

    private void CreateBliank(float o)
    {
        DraggableCreator.Instance.CreateDraggableOnRandomPosition(_draggable[Random.Range(0, _draggable.Length)], transform.position);

        Destroy(gameObject);
    }
}

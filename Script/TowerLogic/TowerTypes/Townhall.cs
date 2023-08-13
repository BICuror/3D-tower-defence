using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Townhall : MonoBehaviour
{
    [SerializeField] private GameObject _timeTower;

    private int _leftCharges; 

    private void Start()
    {     
        FindObjectOfType<CameraRotationController>().SetTarget(transform);

        GetComponent<IDraggable>().Place();
    }

    private void OnDestroy()
    {
        SceneManager.LoadScene(0);
    }
}


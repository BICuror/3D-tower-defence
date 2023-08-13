using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    [SerializeField] private GameObject _pref;
    public bool _s;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10000; i++)
        {
            GameObject g = Instantiate(_pref);

            g.SetActive(_s);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

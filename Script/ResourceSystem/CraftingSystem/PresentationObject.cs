using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public sealed class PresentationObject : DraggableObject
{
    [SerializeField] private MeshFilter _meshFilter;

    public void UpdateVisuals(Mesh mesh)
    { 
        _meshFilter.sharedMesh = mesh;
    }
}

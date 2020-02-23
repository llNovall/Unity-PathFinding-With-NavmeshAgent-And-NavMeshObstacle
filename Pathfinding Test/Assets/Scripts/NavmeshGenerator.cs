using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshGenerator : MonoBehaviour
{
    public static NavmeshGenerator Current;
    public NavMeshSurface NavMeshSurface;

    private void Awake()
    {
        Current = this;
        NavMesh.avoidancePredictionTime = 0;
        NavMeshSurface = gameObject.AddComponent<NavMeshSurface>();
        NavMeshSurface.buildHeightMesh = true;
        NavMeshSurface.BuildNavMesh();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentSpawner : MonoBehaviour
{
    public GameObject AgentPrefab;

    private void Start()
    {
        InvokeRepeating("SpawnAgent", 1, 0.2f);
    }

    public void SpawnAgent()
    {
        NavMeshHit navMeshHit;
        Vector3 dir = Random.insideUnitSphere * 50;
        NavMesh.SamplePosition(dir, out navMeshHit, 200, 1);

        Instantiate(AgentPrefab, navMeshHit.position, Quaternion.identity);
    }

}

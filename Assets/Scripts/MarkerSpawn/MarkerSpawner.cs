using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Biocrowds.Core;
using UnityEngine.AI;

public abstract class MarkerSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public SimulationConfiguration.MarkerSpawnMethod spawnMethod;
    //radius for auxin collide
    public float MarkerRadius = 0.1f;

    //density
    public float MarkerDensity = 0.65f;

    //decay = changes difference in marker radius later on simulation
    public float MarkerDecay = 0.02f;

    protected Transform _auxinsContainer;

    protected float _cellSize;
    protected int _maxMarkersPerCell;

    public Auxin auxinPrefab;

    public abstract IEnumerator CreateMarkers(List<Cell> _cells, List<Auxin> _auxins);

    protected bool HasMarkersNearby(Vector3 test, List<Auxin> others)
    {
        for (int i = 0; i < others.Count; i++)
            if (Vector3.Distance(test, others[i].Position) < MarkerRadius)
                return true;
        return false;
    }

    protected bool HasObstacleNearby(Vector3 test)
    {
        Collider[] hitColliders = Physics.OverlapSphere(test, MarkerRadius + 0.1f, 1 << LayerMask.NameToLayer("Obstacle"));
        return hitColliders.Length > 0 ? true : false;
    }

    protected bool IsOnNavmesh(Vector3 test)
    {
        return NavMesh.SamplePosition(test, out NavMeshHit hit, 0.05f, 1 << NavMesh.GetAreaFromName("Walkable"));
    }
}

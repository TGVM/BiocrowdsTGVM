using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    private Collider _collider;
    private MeshRenderer _meshRenderer;

    [Header("Initial Spawner Settings")]
    public int initialNumberOfAgents;
    public bool initialRemoveWhenGoalReached;
    public List<GameObject> initialAgentsGoalList;
    public List<float> initialWaitList;

    [Header("Repeating Spawner Settings")]
    public float cycleLenght = 1.0f;
    public int quantitySpawnedEachCycle;
    public bool repeatingRemoveWhenGoalReached;
    public List<GameObject> repeatingGoalList;
    public List<float> repeatingWaitList;
    private float cycleCounter = 0.0f;
    private bool cycleReady = false;

    public bool CycleReady { get => cycleReady;  }

    private void Awake()
    {
        if (_collider == null)
            _collider = GetComponent<Collider>();
        if (_meshRenderer == null)
            _meshRenderer = GetComponent<MeshRenderer>();

        cycleCounter = 0.0f;
        cycleReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Debug.Log(GetRandomPoint());
    }

    public void UpdateSpawnCounter(float dt)
    {
        if (cycleLenght == 0.0f || quantitySpawnedEachCycle == 0)
            return;

        cycleCounter += dt;
        if (cycleCounter >= cycleLenght)
        {
            cycleCounter -= cycleLenght;
            cycleReady = true;
        }
    }

    public void ResetCycleReady()
    {
        cycleReady = false;
    }

    public Vector3 GetRandomPoint(float height = 0.0f)
    {
        Vector3 point = new Vector3(
            Random.Range(_collider.bounds.min.x, _collider.bounds.max.x), 
            height,
            Random.Range(_collider.bounds.min.z, _collider.bounds.max.z)
        );
        
        return _collider.ClosestPoint(point);
    }
    public void ShowMesh(bool _show)
    {
        _meshRenderer.enabled = _show;
    }
}

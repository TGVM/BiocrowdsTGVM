using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Biocrowds.Core;
using System.Linq;

public class SceneController : MonoBehaviour
{
    public World world;
    private bool initialized = false;

    public static bool Moshpit = false;
    public static bool Circlepit = false;
    public static bool ShowCells { get; protected set; } = true;
    public static bool ShowAuxins { get; protected set; } = true;
    public static bool ShowSpawnAreas { get; protected set; } = true;
    public static bool ShowAuxinVectors { get; protected set; } = true;

    public static bool ShowNavMeshCorners { get; protected set; } = true;

    [Header("Render Settings")]
    public bool _showCells;
    public bool _showAuxins;
    public bool _showAuxinVector;
    public bool _showSpawnAreas;
    public bool _showNavMeshCorners;

    private void Awake()
    {
        ShowCells = _showCells;
        ShowAuxins = _showAuxins;
        ShowSpawnAreas = _showSpawnAreas;
        ShowAuxinVectors = _showAuxinVector;
        ShowNavMeshCorners = _showNavMeshCorners;
    }
    void Start()
    {
        Debug.Log("Press 1 to load world");
    }

    void Update()
    {
        if (_showCells != ShowCells)
        {
            ShowCells = _showCells;
            world.ShowCellMeshes(_showCells);
        }
        if (_showAuxins != ShowAuxins)
        {
            ShowAuxins = _showAuxins;
            world.ShowAuxinMeshes(_showAuxins);
        }
        if (_showSpawnAreas != ShowSpawnAreas)
        {
            ShowSpawnAreas = _showSpawnAreas;
            List<SpawnArea> _spawners = FindObjectsOfType<SpawnArea>().ToList();
            foreach (SpawnArea s in _spawners)
                s.ShowMesh(_showSpawnAreas);
        }
        if (_showAuxinVector != ShowAuxinVectors) ShowAuxinVectors = _showAuxinVector;
        if (_showNavMeshCorners != ShowNavMeshCorners) ShowNavMeshCorners = _showNavMeshCorners;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reloading Scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && !initialized)
        {
            Debug.Log("Loading World");
            Debug.Log("Press M to start the moshpit");
            Debug.Log("Press C to start the circlepit");
            List<SpawnArea> _spawners = FindObjectsOfType<SpawnArea>().ToList();
            foreach (SpawnArea s in _spawners)
                s.ShowMesh(ShowSpawnAreas);

            initialized = true;
            world.LoadWorld();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            //opens space on sphere area
            Debug.Log("M pressed!");
            Moshpit = !Moshpit;

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //opens space on sphere area
            Debug.Log("C pressed!");
            Circlepit = !Circlepit;

        }

    }
}

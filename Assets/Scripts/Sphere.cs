using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Biocrowds.Core;

public class Sphere : MonoBehaviour
{
    private World _world;
    public World World
    {
        get { return _world; }
        set { _world = value; }
    }

    [Header("Moshpit Goals")]
    //public int initialNumberOfAgents;
    //public bool initialRemoveWhenGoalReached;
    public List<GameObject> moshpitGoalList;
    //public List<float> initialWaitList;

    public List<Agent> agentsList = new List<Agent>();

    public SphereCollider sphereColl;
    public List<Auxin> Auxins;
    public List<Cell> Cells;
    private List<Cell> localCells;

    public List<Agent> Agents;
    private List<Agent> myAgents;

    private bool mpTemp = false;
    private bool moshpit = false;
    private bool loadSph = false;
    private bool ltemp = false;

    //radius for auxin collide
    public float MarkerRadius = 0.1f;

    //density
    public float MarkerDensity = 0.65f;

    protected Transform _auxinsContainer;
    protected float _cellSize;
    protected int _maxMarkersPerCell;

    public Auxin auxinPrefab;


    private void Start()
    {
        World = FindObjectOfType<World>();
    }


    public void LoadSphere()
    {
        Cells = _world.Cells;
        FindLocalCells();
        //FindNearAuxins();
        //Agents = _world.Agents;
        //FindAgents();
    }

    // Update is called once per frame
    void Update()
    {
        loadSph = true;
        if (loadSph != ltemp && World.CellsReady)
        {
            ltemp = loadSph;
            LoadSphere();
        }

        moshpit = SceneController.Moshpit;
        if (moshpit != mpTemp) {
            mpTemp = moshpit;
            Debug.Log("trigger");
            Agents = World.Agents;
            FindAgents();
            //DisableAuxins();
            OpenMoshpit();
            CreateMoreMarkers(localCells, _world.Auxins); //not calling method ???????
        }
    }



    /*plan 2*/
    //1. find out which agents are inside the sphere
    //maybe get agents which distance to next goal is higher than some value

    //finding inside the sphere
    private void FindAgents()
    {
        int agentcount = 0;
        //Debug.Log("Finding");
        myAgents = new List<Agent>();
        for (int i = 0; i < Agents.Count; i++)
        {
            float dist = Vector3.Distance(Agents[i].transform.position, this.transform.position);
            if (dist <= 5)
            {
                //Debug.Log("found" + Agents[i].name);
                myAgents.Add(Agents[i]);
                agentcount++;
            }
        }
        Agents = myAgents;
        Debug.Log("agentes afetados: " + agentcount);
    }

    //finding far from goal
    //maybe get agents far from goal and outside the sphere
    //send them farther from goal
    private void FindAgents2()
    {
        //Debug.Log("Finding");
        myAgents = new List<Agent>();
        for (int i = 0; i < Agents.Count; i++)
        {
            float dist = Vector3.Distance(Agents[i].transform.position, Agents[i].Goal.transform.position);
            if (dist > 2)
            {
                //Debug.Log("found" + Agents[i].name);
                myAgents.Add(Agents[i]);
            }
        }
        Agents = myAgents;
    }

    //2. once the trigger moshpit is activated find new goals near the sphere for those agents 
    private void OpenMoshpit(){
        for (int i=0; i<Agents.Count; i++)
        {
            GameObject newGoal = null;

            //nearest method
            float minDist = Mathf.Infinity;
            Vector3 currentPos = Agents[i].transform.position;
            foreach (GameObject g in moshpitGoalList)
            {
                float dist = Vector3.Distance(g.transform.position, currentPos);
                if (dist < minDist)
                {
                    newGoal = g;
                    minDist = dist;
                }
            }
            // Debug.Log(Agents[i].name + " new goal " + newGoal.name);
            //create the "panic" system for agents to close the gaps between them
            // Agents[i].agentRadius = 0.5f;
            Agents[i].agentRadius = Agents[i].agentRadius / 4;
            Agents[i].AddGoal(newGoal);
            Agents[i].SkipGoal();
            Agents[i].SetColorToRed();
            

        }
        Debug.Log("mosh begins");
    }

    //2.5 maybe add MORE auxins in the sphere area

    private void FindLocalCells()
    {
        localCells = new List<Cell>();
        for (int i = 0; i < Cells.Count; i++)
        {
            float dist = Vector3.Distance(Cells[i].transform.position, this.transform.position);
            if (dist < 5)
            {
                localCells.Add(Cells[i]);
            }
        }
        Debug.Log("local cells found");
    }

    public IEnumerator CreateMoreMarkers(List<Cell> cells, List<Auxin> auxins)
    {
        Debug.Log("create markers start");
        _auxinsContainer = new GameObject("Markers").transform;
        _cellSize = cells[0].transform.localScale.x;

        int initsize = auxins.Count;

        //_maxMarkersPerCell = Mathf.RoundToInt(MarkerDensity / (MarkerRadius * MarkerRadius));

        // Generate a number of markers for each Cell
        for (int c = 0; c < cells.Count; c++)
        {

            StartCoroutine(PopulateLocalCell(cells[c], auxins, c));
        }

        int endsize = auxins.Count;

        Debug.Log(initsize + " -> " + endsize);

        yield break;
    }

    private IEnumerator PopulateLocalCell(Cell cell, List<Auxin> auxins, int cellIndex)
    {
        Debug.Log("populate cell start");

        float cellHalfSize = (_cellSize / 2.0f) * (1.0f - (MarkerRadius / 2f));

        // Set this counter to break the loop if it is taking too long (maybe there is no more space)
        int _tries = 0;

        _maxMarkersPerCell = Mathf.RoundToInt(MarkerDensity / (MarkerRadius * MarkerRadius));


        for (int i = 0; i < _maxMarkersPerCell; i++)
        {
            // If counter is above maxMarkers * 5, breaks the sequence for this Cell
            if (_tries > _maxMarkersPerCell * 5)
                break;

            // Candidate position for new Marker
            float x = Random.Range(-cellHalfSize, cellHalfSize);
            float z = Random.Range(-cellHalfSize, cellHalfSize);
            Vector3 targetPosition = new Vector3(x, 0f, z) + cell.transform.position;

            //if (HasObstacleNearby(targetPosition) || HasMarkersNearby(targetPosition, cell.Auxins)
            //    || !IsOnNavmesh(targetPosition))
            //{
            //    _tries++;
            //    i--;
            //    continue;
            //}

            // Creates new Marker and sets its data
            Auxin newMarker = Instantiate(auxinPrefab, targetPosition, Quaternion.identity, _auxinsContainer);
            newMarker.transform.localScale = Vector3.one * MarkerRadius;
            newMarker.name = "NewMarker [" + cellIndex + "][" + i + "]";
            newMarker.Cell = cell;
            newMarker.Position = targetPosition;
            newMarker.ShowMesh(SceneController.ShowAuxins);

            auxins.Add(newMarker);
            cell.Auxins.Add(newMarker);



            // Reset the tries counter
            _tries = 0;
        }
        yield break;
    }

    //OBS: the way it works now, if there's not many agents behind the back part of the circle, they open the moshpit just fine,
    //because they have space to move


    //3. after most of the agents are around the sphere start sending some of them to the middle
    //3.5 test collision(?) between them and send them in opposite directions






    /*plan 1*/
    //1. find out which cells collide with the sphere
    //private void FindLocalCells()
    //{
    //    localCells = new List<Cell>();
    //    for(int i=0; i<Cells.Count; i++)
    //    {
    //        float dist = Vector3.Distance(Cells[i].transform.position, this.transform.position);
    //        if (dist < 5)
    //        {
    //            localCells.Add(Cells[i]);
    //        }
    //    }
    //}


    //2. search within those cells which auxins are inside sphere and register those auxins
    //public void FindNearAuxins()
    //{
    //    //clear them all, for obvious reasons
    //    Auxins.Clear();

    //    for(int i = 0; i < localCells.Count; i++)
    //    {
    //        //get all auxins on my cell
    //        List<Auxin> cellAuxins = localCells[i].Auxins;

    //        //iterate all cell auxins to check distance between auxins and sphere
    //        for (int j = 0; j < cellAuxins.Count; j++)
    //        {
    //            //see if the distance between this sphere and this auxin is smaller than the actual value, and inside agent radius
    //            float dist = Vector3.Distance(cellAuxins[j].transform.position, this.transform.position);
    //            if (dist < 4)
    //            {
    //                Auxins.Add(cellAuxins[j]);
    //            }
    //        }
    //    }
    //}


    //4. have a trigger activated with world.moshpit
    //  On update


    //5. disable registered auxins when moshpit is true
    //private void DisableAuxins()
    //{
    //    foreach(Auxin a in Auxins)
    //    {
    //        a.gameObject.SetActive(!moshpit);
    //        a.isActive = !moshpit;
    //    }

    //}



}

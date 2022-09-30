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


    //public List<Auxin> Auxins;
    //public List<Cell> Cells;
    //private List<Cell> localCells;

    public List<Agent> Agents;
    private List<Agent> myAgents;

    private bool mpTemp = false;
    private bool moshpit = false;
    private bool loadSph = false;
    private bool ltemp = false;


    private void Start()
    {
        World = FindObjectOfType<World>();
    }

    public void LoadSphere()
    {
        //Cells = _world.Cells;
        //FindLocalCells();
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
            //LoadSphere();
        }

        moshpit = SceneController.Moshpit;
        if (moshpit != mpTemp) {
            mpTemp = moshpit;
            Debug.Log("trigger");
            Agents = World.Agents;
            FindAgents();
            //DisableAuxins();
            OpenMoshpit();
        }
    }



    /*plan 2*/
    //1. find out which agents are inside the sphere
    private void FindAgents()
    {
        myAgents = new List<Agent>();
        for (int i = 0; i < Agents.Count; i++)
        {
            float dist = Vector3.Distance(Agents[i].transform.position, this.transform.position);
            if (dist < 3)
            {
                myAgents.Add(Agents[i]);
            }
        }
        Agents = myAgents;
    }

    //2. once the trigger moshpit is activated sort new goals near the sphere for those agents (random goals or nearest goals?)
    //random does not work
    private void OpenMoshpit(){
        
        foreach (Agent a in Agents)
        {

            GameObject newGoal = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = a.transform.position;
            foreach (GameObject g in moshpitGoalList)
            {
                float dist = Vector3.Distance(g.transform.position, currentPos);
                if (dist < minDist)
                {
                    newGoal = g;
                    minDist = dist;
                }
            }
            a.goalsList.Add(newGoal);
            a.SkipGoal();


            //random method
            //int num = Random.Range(0, moshpitGoalList.Count);
            //newGoal = moshpitGoalList[num];
            //a.goalsList.Add(newGoal);
            //a.SkipGoal();
        }
        
        
    }

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

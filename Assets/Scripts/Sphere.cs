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




    public List<Auxin> Auxins;
    public List<Cell> Cells;
    private List<Cell> localCells;

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
        
        Cells = _world.Cells;
        FindLocalCells();
        FindNearAuxins();
    }

    // Update is called once per frame
    void Update()
    {
        loadSph = true;
        if(loadSph != ltemp && World.CellsReady)
        {
            ltemp = loadSph;
            LoadSphere();
        }

        moshpit = SceneController.Moshpit;
        if (moshpit != mpTemp) {
            mpTemp = moshpit;
            Debug.Log("trigger");
            DisableAuxins();
        }
    }

    //1. find out which cells collide with the sphere
    private void FindLocalCells()
    {
        localCells = new List<Cell>();
        for(int i=0; i<Cells.Count; i++)
        {
            float dist = Vector3.Distance(Cells[i].transform.position, this.transform.position);
            if (dist < 5)
            {
                localCells.Add(Cells[i]);
            }
        }
    }


    //2. search within those cells which auxins are inside sphere and register those auxins
    public void FindNearAuxins()
    {
        //clear them all, for obvious reasons
        Auxins.Clear();

        for(int i = 0; i < localCells.Count; i++)
        {
            //get all auxins on my cell
            List<Auxin> cellAuxins = localCells[i].Auxins;

            //iterate all cell auxins to check distance between auxins and sphere
            for (int j = 0; j < cellAuxins.Count; j++)
            {
                //see if the distance between this sphere and this auxin is smaller than the actual value, and inside agent radius
                float dist = Vector3.Distance(cellAuxins[j].transform.position, this.transform.position);
                if (dist < 4)
                {
                    Auxins.Add(cellAuxins[j]);
                }
            }
        }
    }

    
    //4. have a trigger activated with world.moshpit
    //  On update


    //5. disable registered auxins when moshpit is true
    private void DisableAuxins()
    {
        foreach(Auxin a in Auxins)
        {
            a.gameObject.SetActive(!moshpit);
            a.isActive = !moshpit;
        }

    }



}

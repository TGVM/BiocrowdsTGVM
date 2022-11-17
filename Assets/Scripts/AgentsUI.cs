using Biocrowds.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentsUI : MonoBehaviour
{
    private World _world;
    public World World
    {
        get { return _world; }
        set { _world = value; }
    }

    public Text myText;

    // Start is called before the first frame update
    void Start()
    {
        World = FindObjectOfType<World>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(World.Agents.Count.ToString());
        myText.text = World.Agents.Count.ToString();
    }
}

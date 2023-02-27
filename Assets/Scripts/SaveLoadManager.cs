using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Biocrowds.Core;

public class SaveLoadManager
{
    /*
    links:
    Binary: https://www.youtube.com/watch?v=sWWZZByVvlU&ab_channel=BoardToBitsGames
    XML: https://www.youtube.com/watch?v=6vl1IYMpwVQ&ab_channel=BoardToBitsGames
        Decidir o que deve ficar salvo
        World:
            Max_agents, agent_radius, auxin_radius, goal_distance_threshold, reflect_threshold(min/max), number_ag_mosh

        Agent:
            transform.position, agent_radius, velocity(vector3) moshpit, reverse, reflect, reflect threshold(min/max), goal, goal index,goal list, goal dist threshold
            
            Ver se não vai precisar colocar tudo || ver se vai precisar/consegue carregar modelo

        Marker:
            transform.position

        Sphere:
            transofrm.position, radius, moshpitGoal, marker Radius, marker density, lesserDist, timeToStart
    
    */

    /*
     ref de World para pegar agents atuais e para carregar agents salvos

    input de save:
    cria arquivo / reset em arquivo já existente
    salva parâmetros dos auxins: pos
    salva parâmetros dos agents: pos, goal, auxins

    input de load:
    busca arquivo
    cria auxins com parâmetros salvos
    cria agentes com parâmetros salvos

     
     */

    private World _world;
    public World World
    {
        get { return _world; }
        set { _world = value; }
    }

    private void Start()
    {
        //World = FindObjectOfType<World>();

       
    }

}

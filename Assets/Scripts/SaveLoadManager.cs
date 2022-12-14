using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager
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

}

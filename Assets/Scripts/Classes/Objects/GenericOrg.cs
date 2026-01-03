using UnityEngine;

public class GenericOrg
{
    //Some info about an org is hidden until they are discovered by the player
    bool discovered = false;
    // How important the org is on the world stage
    //? Maybe have it so random events can happen aside from those caused by proposals, and the events are decided by the influence level (and maybe current scenario?)
    int influence = 25;

    // Stores the stat bar info 
    //TODO figure out how to use this for the foundation too, as it has multiple stat bars

    //Store world events that can be run based on the world map stage of the game and potentially some level of "influence" that GoI has on the map?
    //These world events are unlocked during the proposal stage
    //Players can interact with the world events, which can lead to other world events to happen (also try tie this to proposal unlocking... somehow? Maybe add another prereq section)


}   

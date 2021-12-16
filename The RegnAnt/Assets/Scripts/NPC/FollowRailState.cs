using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRailState : State
{
    private AntFSM _ant;
    public FollowRailState(string name, AntFSM ant) : base(name)
    {
        _ant = ant;
    }

    public override void Enter()
    {
       Debug.Log("Follow rail state");
        
    }

    public override void Tik()
    {        
        
    }

    public override void Exit()
    {
        
    }
    
}

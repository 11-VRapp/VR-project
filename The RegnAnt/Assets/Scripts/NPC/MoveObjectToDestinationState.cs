using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectToDestinationState : State
{
    private AntFSM _ant;
    public MoveObjectToDestinationState(string name, AntFSM ant) : base(name)
    {
        _ant = ant;
    }

    public override void Enter()
    {
        /*_guard.StopAgent(false);
        _guard.Renderer.material.color = _guard.OriginalColor;*/
    }

    public override void Tik()
    {        
        _ant.moveToDestination();
        
    }

    public override void Exit()
    {
        
    }
    
}


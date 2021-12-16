using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : State
{
    private AntFSM _ant;
    public WanderState(string name, AntFSM ant) : base(name)
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
        _ant.SetRandomPointDestination();
        //_ant.groundCheck();
        _ant.LookAround();
    }

    public override void Exit()
    {
        
    }
    
}

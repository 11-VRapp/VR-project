using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadObjectState : State
{
    private AntFSM _ant;
    public LoadObjectState(string name, AntFSM ant) : base(name)
    {
        _ant = ant;
    }

    public override void Enter()
    {
        Debug.Log("Hehehehe");
        /*_guard.StopAgent(false);
        _guard.Renderer.material.color = _guard.OriginalColor;*/
    }

    public override void Tik()
    {        
        _ant.moveToFood();
       
    }

    public override void Exit()
    {
        
    }
    
}
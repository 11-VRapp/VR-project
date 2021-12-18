using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    private string _name;

    public string Name => _name;

    protected State(string name)
    {
        _name = name;
    }
    public abstract void Enter();
    public abstract void Tik();
    public abstract void Exit();
}

public class WanderState : State
{
    private AntFSM _ant;
    public WanderState(string name, AntFSM ant) : base(name) => _ant = ant;
    

    public override void Enter()
    {
        /*_guard.StopAgent(false);
        _guard.Renderer.material.color = _guard.OriginalColor;*/
    }

    public override void Tik()
    {        
        _ant.SetRandomPointDestination();        
        _ant.LookAround();
    }

    public override void Exit()
    {
        
    }
    
}


public class MoveFoodToNestState : State
{
    private AntFSM _ant;
    public MoveFoodToNestState(string name, AntFSM ant) : base(name) { _ant = ant; }
    public override void Enter()
    {
        
    }

    public override void Tik() => _ant.moveToDestination();        
    

    public override void Exit()
    {
        
    }
    
}

public class LoadFoodState : State
{
    private AntFSM _ant;
    public LoadFoodState(string name, AntFSM ant) : base(name) => _ant = ant; 

    public override void Enter()
    {        
        /*_guard.StopAgent(false);
        _guard.Renderer.material.color = _guard.OriginalColor;*/
    }

    public override void Tik()
    {        
        _ant.moveToFood();
       
    }

    public override void Exit() =>  _ant.objectToLoad = null;
    
    
}


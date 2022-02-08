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
        _ant.following = false;
        _ant.objectToLoad = null;
        _ant.pheromoneTrace = null;
        _ant.resetHeadPosition();
    }

    public override void Tik()
    {
        _ant.SetRandomPointDestination();
        _ant.LookAround();
    }

    public override void Exit() { }
}


public class LoadFoodState : State
{
    private AntFSM _ant;
    public LoadFoodState(string name, AntFSM ant) : base(name) => _ant = ant;

    public override void Enter() { _ant.StartCoroutine(_ant.moveToFood()); }

    public override void Tik() { } //=> _ant.moveToFood();


    public override void Exit() { _ant.returnBack = false; }//=> _ant.grabFood();


}

public class FollowPheromoneTraceState : State
{
    private AntFSM _ant;
    public FollowPheromoneTraceState(string name, AntFSM ant) : base(name) => _ant = ant;

    public override void Enter()
    {
        _ant.StartCoroutine(_ant.followPheromoneTraceToFood());
    }

    public override void Tik() { }

    public override void Exit() { }

}

public class SpawnNewPheromoneTraceState : State
{
    private AntFSM _ant;
    public SpawnNewPheromoneTraceState(string name, AntFSM ant) : base(name) => _ant = ant;

    public override void Enter() { _ant.newPheromoneTraceHandler(); }

    public override void Tik() { }

    public override void Exit() { _ant.StopCoroutine(_ant.spawnPheromoneCoroutine); _ant.destroyFood(); }
}

public class FollowPheromoneTraceToNestState : State
{
    private AntFSM _ant;
    public FollowPheromoneTraceToNestState(string name, AntFSM ant) : base(name) => _ant = ant;

    public override void Enter() { _ant.StartCoroutine(_ant.followPheromoneTraceToNest()); }

    public override void Tik()
    {
        // _ant.followPheromoneTraceToNestState();
    }

    public override void Exit() { _ant.destroyFood(); }

}

public class FollowPlayerState : State
{
    private AntFSM _ant;
    public FollowPlayerState(string name, AntFSM ant) : base(name) => _ant = ant;

    public override void Enter() { }

    public override void Tik()
    {
        _ant.followPlayer();
    }

    public override void Exit() { }

}



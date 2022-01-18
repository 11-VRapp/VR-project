using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RunState : State
{
    private spiderFSM _spider;
    public RunState(string name, spiderFSM spider) : base(name) => _spider = spider;


    public override void Enter()
    {
       _spider.SetRandomPointDestination();
       _spider.SetSemaphoreLegsCounter(0);
    }

    public override void Tik()
    {
        _spider.RotateTowards();
    }

    public override void Exit() { }
}


public class AttackState : State
{
    private spiderFSM _spider;
    public AttackState(string name, spiderFSM spider) : base(name) => _spider = spider;

    public override void Enter() => _spider.Attack();

    public override void Tik()
    {
        _spider.CheckPlayerNotTooClose();
    }


    public override void Exit() { }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiderEyes : Damagable
{
    [Header("Eyes Attributes")]
    [SerializeField] private float damage = 20f;
    [SerializeField] private spiderLife _sl;
    public override void Hit()
    {
        _sl.setDamage(damage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public PheromoneRailTrace phTrace;

    //[SerializeField] private int pieces;

    private void Start()
    {
        phTrace = new PheromoneRailTrace(); //necessario? Da errore con il new...
    }

    public void DestroyPhTrace() => Destroy(this.gameObject);  //destroy when trace has disappeared or problem with phTrace variable
}

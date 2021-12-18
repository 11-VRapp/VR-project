using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public PheromoneRailTrace phTrace;

    //[SerializeField] private int pieces;

    private void Start()
    {
        phTrace = new PheromoneRailTrace();
    }
}

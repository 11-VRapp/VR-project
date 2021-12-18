using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneRailTrace : MonoBehaviour
{        
    public LinkedList<PheromoneRailPoint> list;       

    //public FoodManager foodRelativeTo;

    void Start()
    {
        list = new LinkedList<PheromoneRailPoint>();
    }   

    public LinkedListNode<PheromoneRailPoint> getNodeByPoint(PheromoneRailPoint point){return list.Find(point);}
    public LinkedListNode<PheromoneRailPoint> getHeadNode() { return list.First;}
    public LinkedListNode<PheromoneRailPoint> getTailNode() { return list.Last;}
    public void pushPointToTrace(PheromoneRailPoint point) {list.AddLast(point);}
    public LinkedListNode<PheromoneRailPoint> getNextPoint(LinkedListNode<PheromoneRailPoint> curNode) { return curNode.Next; }   
    public LinkedListNode<PheromoneRailPoint> getPrevPoint(LinkedListNode<PheromoneRailPoint> curNode) { return curNode.Previous; }   

    
}

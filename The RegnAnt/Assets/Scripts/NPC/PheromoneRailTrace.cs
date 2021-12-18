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

    //public PheromoneRailPoint getHeadNode() { return list.First.Value;}

    //public void pushPointToTrace(PheromoneRailPoint point) => list.AddLast(point);

    /*public PheromoneRailPoint getNextPoint(PheromoneRailPoint actualPoint) //in order from base to food
    {
        var curNode = list.Find(actualPoint);  

        if(curNode.Next == null)
            return null;             
        LinkedListNode<PheromoneRailPoint> nextNode = curNode.Next;

        return nextNode.Value;
    }   */

    public LinkedListNode<PheromoneRailPoint> getNodeByPoint(PheromoneRailPoint point){return list.Find(point);}
    public LinkedListNode<PheromoneRailPoint> getHeadNode() { return list.First;}
    public LinkedListNode<PheromoneRailPoint> getTailNode() { return list.Last;}
    public void pushPointToTrace(LinkedListNode<PheromoneRailPoint> point) => list.AddLast(point);
    public LinkedListNode<PheromoneRailPoint> getNextPoint(LinkedListNode<PheromoneRailPoint> curNode) { return curNode.Next; }   

    public LinkedListNode<PheromoneRailPoint> getPrevPoint(LinkedListNode<PheromoneRailPoint> curNode) { return curNode.Previous; }   

    /*public PheromoneRailPoint getPrevPoint(PheromoneRailPoint actualPoint) //in order from base to food
    {       
        var curNode = list.Find(actualPoint); 

        if(curNode.Previous == null)
            return null;        
        LinkedListNode<PheromoneRailPoint> nextNode = curNode.Previous;
        return nextNode.Value;
    }  */
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StageFollow : MonoBehaviour
{
    public GameObject imageBox;
    public Text nameText;
    public Text answer;
    public string answer_yes;
    public string answer_no;

    private Dictionary<string, string[]> negative_answers_byState = new Dictionary<string, string[]>();

    void Start()
    {
        string[] loadfoodState = { 
            "Scusami ma non posso che sto prendendo del cibo", 
            "Non ora! Non hai sentito quel cibo!?",
            "Quel cibo non aspetta nessuno",
            "Veramente avrei da fare al momento. Senti lì che bel cibo",
            "Aspetta! Mi sembra di percepire qualcosa di buono nei dintorni!",
            "La colonia ha fame, non posso seguirti adesso..." };
        negative_answers_byState.Add("loadFood", loadfoodState);

        string[] followPheromoneTraceState = { 
            "Scusami ma sono curiosa di vedere dove mi porti questa scia", 
            "Non senti che una nostra sorella ci ha indicato la via?",
            "Non ora. Sembra che qualcuna abbia trovato del cibo",
            "Non ho tempo adesso... sto seguendo questa traccia",
            "Che ne dici di venire con me verso il cibo?",
            "Andiamo a vedere se c'è davvero cibo?"};
        negative_answers_byState.Add("followPheromoneTrace", followPheromoneTraceState);

        string[] spawnNewPheromoneTraceState = { 
            "Non interrompermi che sto lasciando feromoni per le nostre sorelle", 
            "Largo, largo!",
            "Devo segnalare a tutte che abbiamo trovato dell'ottimo cibo!",
            "Le altre sorelle saranno felici di sapere che c'è altro cibo",
            "Avvisiamo tutti che c'è da mangiare qua intorno!",
            "Spostati che devo avvertire che ho trovato cibo" };
        negative_answers_byState.Add("spawnNewPheromoneTrace", spawnNewPheromoneTraceState);

        string[] FollowPheromoneTraceToNestState = { 
            "Perché non ci aiuti a raccogliere il cibo?", 
            "Che bello, si banchetta!",
            "Ora non posso. Devo portare il cibo alla colonia",
            "Devi sempre aver fiducia nelle nostre sorelle. Chissà che buono", 
            "Ma mica ho tempo per seguirti. Devo tornare al nido"};
        negative_answers_byState.Add("followPheromoneTraceToNest", FollowPheromoneTraceToNestState);
    }

    public string getPhraseByState(string state)
    {
        string[] arr = negative_answers_byState[state];
        int index = Random.Range(0, arr.Length - 1);
        return arr[index];
    }



}

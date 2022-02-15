using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

	public string name;

	[TextArea(3, 10)]
	public string fraseIniziale;

	public string stage1_CourtesyQuestion;

	public bool esterno;

	[TextArea(3, 10)]
	public string[] domande;

	[TextArea(3, 10)]
	public string[] risposte;

	public string follow_yes = "Va bene";
	public string follow_no;

	public Dialogue(string name, string fraseIniziale, string courtesyQuestion, bool esterno, string[] domande, string[] risposte)
	{
		this.name = name;
		this.fraseIniziale = fraseIniziale;
		stage1_CourtesyQuestion = courtesyQuestion;
		this.esterno = esterno;
		this.domande = domande;
		this.risposte = risposte;
	}

}

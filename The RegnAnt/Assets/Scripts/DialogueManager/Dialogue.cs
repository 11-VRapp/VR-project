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

	public string follow_yes;
	public string follow_no;

}

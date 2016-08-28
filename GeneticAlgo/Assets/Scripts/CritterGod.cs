using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CritterGod : MonoBehaviour {

	public GameObject critterPrefab;

	public int numberPerGeneration = 30;
	public IList<Critter> critters;

	// Use this for initialization
	void Start () {
		critters = new List<Critter> ();
		for (int i = 0; i < numberPerGeneration; i++){
			Critter c = Instantiate(critterPrefab).GetComponent<Critter>();
			critters.Add(c);
		}
	}

	void Update () {
	
	}

	public void RemoveCritter(GameObject CritterToRemove){

	}


}

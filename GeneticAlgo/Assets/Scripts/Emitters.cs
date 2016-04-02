using UnityEngine;
using System.Collections;

public class Emitters : MonoBehaviour {

	public enum Types {
		Food,
		Poison
	}

	public float strength = 1.0f;
	public EmitterType emitterType = new EmitterType();
	public Types type;
	
	void Start () {

	}

	void Update () {
		
	}
}

public class EmitterType{
	public void Init(){}
	public void OnCollect(){}
}

public class EmitterFood : EmitterType{
	
}

public class EmitterPoison : EmitterType{
	
}
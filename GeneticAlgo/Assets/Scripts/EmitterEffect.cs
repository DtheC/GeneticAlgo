using UnityEngine;
using System.Collections;

public class EmitterEffect : MonoBehaviour {

	public float strength = 10f;
	public Color emitColor = new Color(1f,0.5f,0.01f, 0.1f);
	public float emitSpeed = 0.3f;

	private float _currentsize = 0f;
	private GameObject _effectMesh;

	// Use this for initialization
	void Start () {
		_effectMesh = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		_effectMesh.transform.position = gameObject.transform.position;
		_effectMesh.transform.parent = gameObject.transform;

		//Material m = new Material (Shader.Find("Standard"));
		//m. = emitColor;
		_effectMesh.GetComponent<MeshRenderer> ().material.color = emitColor;

		updateEffectSize (new Vector3(0f,0f,0f));
	}

	void updateEffectSize(Vector3 v){
		_effectMesh.transform.localScale = v;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_currentsize += emitSpeed;
		if (_currentsize >= strength) {
			_currentsize = 0;
		}
		updateEffectSize (new Vector3 (_currentsize, _currentsize, _currentsize));
	}
}

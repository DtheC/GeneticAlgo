using UnityEngine;
using System.Collections;

public class Critter : MonoBehaviour {

	public float visionRadius = 10.0f;
	
	private NeuralNetwork brain;
	private float[] _inputs;

	private bool _canSeeEnemy = false;
	private float _closestEnemyDegreesNormalised = 0f;

	//Movement
	private float _movementDirectionDegreesNormalised = 0.00f;

	void Start () {
		brain = new NeuralNetwork ();
		//Input: Direction travelling, Enemy in sight, Direction of Enemy
		_inputs = new float[3];
		brain.Initialise (3, 12, 1);
		brain.SetLearningRate (0.2f);
		brain.SetMomentum (true, 0.9f);


		_movementDirectionDegreesNormalised = Random.Range (0.00f, 1.00f);
	}

	void Update () {
		FindClosestEnemy ();

		_inputs [0] = _movementDirectionDegreesNormalised;
		_inputs [1] = System.Convert.ToSingle(_canSeeEnemy);
		_inputs [2] = _closestEnemyDegreesNormalised;

		if (Input.GetKey("left")){
			_movementDirectionDegreesNormalised -= 0.01f;
		}

		if (Input.GetKey("right")){
			_movementDirectionDegreesNormalised += 0.01f;
		}

		SetBrainInputs ();
		brain.FeedForward ();
		_movementDirectionDegreesNormalised = brain.GetOutput (0);
		Move ();
	}

	void SetBrainInputs(){
		for (int i = 0; i < _inputs.Length; i++) {
			brain.SetInput (i, _inputs[i]);
		}
	}

	void Move(){
		transform.gameObject.GetComponent<Rigidbody> ().velocity =transform.up;
		transform.RotateAround (transform.position, new Vector3(0f,1f,0f), _movementDirectionDegreesNormalised-0.5f);
	}

	void FindClosestEnemy(){
		Collider[] hitColliders = Physics.OverlapSphere (gameObject.transform.position, visionRadius);

		if (hitColliders.Length == 0) {
			_canSeeEnemy = false;
			return;
		}

		_canSeeEnemy = true;
		_closestEnemyDegreesNormalised = EnemyPositionToNormalisedDegrees (hitColliders [0].gameObject.transform.position);

		int i = 1;
		while (i < hitColliders.Length) {
			float tempDist = EnemyPositionToNormalisedDegrees(hitColliders [i].gameObject.transform.position);
			if (tempDist < _closestEnemyDegreesNormalised){
				_closestEnemyDegreesNormalised = tempDist;
			}
			i++;
		}
	}

	void OnCollisionEnter(Collision collision) {

	}
	
	public float EnemyPositionToNormalisedDegrees(Vector3 EnemyPosition){
		return Vector3.Angle (transform.position, EnemyPosition) / 360;
	}
}
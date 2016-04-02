﻿using UnityEngine;
using System.Collections;

public class Critter : MonoBehaviour {

	public float visionRadius = 10.0f;
	
	private NeuralNetwork brain;
	private float[] _inputs;

	private bool _canSeeEnemy = false;
	private float _closestEnemyDegreesNormalised = 0f;

	//Movement
	private float _movementDirectionDegreesNormalised = 0f;

	void Start () {
		brain = new NeuralNetwork ();
		//Input: Direction travelling, Enemy in sight, Direction of Enemy
		_inputs = new float[3];
		brain.Initialise (3, 12, 1);
		brain.SetLearningRate (0.2f);
		brain.SetMomentum (true, 0.9f);
	}

	void Update () {
		FindClosestEnemy ();

		_inputs [0] = _movementDirectionDegreesNormalised;
		_inputs [1] = System.Convert.ToSingle(_canSeeEnemy);
		_inputs [2] = _closestEnemyDegreesNormalised;
		Move ();
	}

	void Move(){
		transform.gameObject.GetComponent<Rigidbody> ().velocity =transform.up;
		transform.RotateAround (transform.position, transform.up, _movementDirectionDegreesNormalised * 360);
//		_movementDirectionDegreesNormalised = Random.Range (0f, 1.00f);
//		transform.gameObject.GetComponent<Rigidbody> ().MoveRotation(Quaternion.AngleAxis (_movementDirectionDegreesNormalised * 360, transform.right));
			

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
		//Die on hit
	}
	
	public float EnemyPositionToNormalisedDegrees(Vector3 EnemyPosition){
		return Vector3.Angle (transform.position, EnemyPosition) / 360;
	}
}
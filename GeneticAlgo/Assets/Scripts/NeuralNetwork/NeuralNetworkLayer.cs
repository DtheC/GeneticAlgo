using UnityEngine;
using System.Collections;

public class NeuralNetworkLayer {

	public int NumberOfNodes;
	public int NumberOfChildNodes;
	public int NumberOfParentNodes;

	public float[] NeuronValues;
	public float[] DesiredValues;

	public float LearningRate;
	public bool LinearOutput;
	public bool UseMomentum;
	public float MomentumFactor;

	private float[,] _weights;
	private float[,] _weightChanges;
	private float[] _errors;
	private float[] _biasWeights;
	private float[] _biasValues;

	private NeuralNetworkLayer _parentLayer;
	private NeuralNetworkLayer _childLayer;

	public NeuralNetworkLayer(){
		_parentLayer = null;
		_childLayer = null;
		LinearOutput = false;
		UseMomentum = false;
		MomentumFactor = 0.9f;
	}

	public void SetLayers(NeuralNetworkLayer[] ParentChild){
		if (ParentChild [0] != null) {
			_parentLayer = ParentChild[0];
		}

		if (ParentChild [1] != null) {
			_childLayer = ParentChild[1];
		}
	}

	public void Initialise(int NumNodes, NeuralNetworkLayer[] ParentChild){
		//Allocate everything
		NumberOfNodes = NumNodes;

		NeuronValues = new float[NumberOfNodes];
		DesiredValues = new float[NumberOfNodes];
		_errors = new float[NumberOfNodes];

		if (ParentChild [0] != null) {
			_parentLayer = ParentChild[0];
		}
		
		if (ParentChild [1] != null) {
			_childLayer = ParentChild [1];

			_weights = new float[NumberOfNodes, NumberOfChildNodes];
			_weightChanges = new float[NumberOfNodes, NumberOfChildNodes];

			_biasValues = new float[NumberOfChildNodes];
			_biasWeights = new float[NumberOfChildNodes];
		} else {
			_weights = null;
			_biasValues = null;
			_biasWeights = null;
			_weightChanges = null;
		}

		//0 out everything
		for (int i = 0; i < NumberOfNodes; i++) {
			NeuronValues[i] = 0;
			DesiredValues[i] = 0;
			_errors[i] = 0;

			if (_childLayer != null){
				for (int j = 0; j < NumberOfChildNodes; j++) {
					_weights[i, j] = 0;
					_weightChanges[i, j] = 0;
				}
			}
		}

		//Init the bias values and weights
		if (_childLayer != null) {
			for (int j = 0; j < NumberOfChildNodes; j++) {
				_biasValues[j] = -1;
				_biasWeights[j] = 0;
			}
		}
	}

	public void RandomiseWeights(){
		for (int i = 0; i < NumberOfNodes; i++){
			for (int j = 0; j < NumberOfChildNodes; j++){
				_weights[i, j] = Random.Range(-1f, 1f);
				_biasWeights[j] = Random.Range(-1f, 1f);
			}
		}
	}

	public void CalculateErrors(){
		float sum = 0;
		if (_childLayer == null){ // Output Layer
			for (int i = 0; i < NumberOfNodes; i++){
				_errors[i] = (DesiredValues[i] - NeuronValues[i]) * NeuronValues[i] * (1 - NeuronValues[i]); 
			}
		} else if (_parentLayer == null){ // Input Layer
			for (int i = 0; i < NumberOfNodes; i++){
				_errors[i] = 0; 
			}
		} else { // Hidden Layer
			for (int i = 0; i < NumberOfNodes; i++){
				sum = 0;
				for (int j = 0; j < NumberOfChildNodes; j++){
					sum+= _childLayer._errors[j] * _weights[i, j]; 
				}
				_errors[i] = sum * NeuronValues[i] * (1-NeuronValues[i]);
			}
		}
	}
	
	public void AdjustWeights(){
		float dw = 0;
		if (_childLayer != null){
			for (int i = 0; i < NumberOfNodes; i++){
				for (int j = 0; j < NumberOfChildNodes; j++){
					dw = LearningRate * _childLayer._errors[j] * NeuronValues[i];
					if (UseMomentum){
						_weights[i, j] += dw + MomentumFactor * _weightChanges[i, j];
						_weightChanges[i, j] = dw;
					} else {
						_weights[i, j] += dw; 
					}
				}
			}
			for (int j=0; j < NumberOfChildNodes; j++){
				_biasWeights[j] += LearningRate * _childLayer._errors[j] * _biasValues[j];
			}
		}
	}
	
	public void CalculateNeuronValues(){
		if (_parentLayer != null){
			for (int i = 0; i < NumberOfNodes; i++){
				float x = 0;
				for (int j = 0; j < NumberOfParentNodes; j++){
					x += _parentLayer.NeuronValues[j] * _parentLayer._weights[j, i]; 
				}
				x += _parentLayer._biasValues[i] * _parentLayer._biasWeights[i];
				if ((_childLayer == null) && LinearOutput){
					NeuronValues[i] = x;
				} else {
					NeuronValues[i] = 1/(1+Mathf.Exp(-x));
				}
			}
		}
	}

}

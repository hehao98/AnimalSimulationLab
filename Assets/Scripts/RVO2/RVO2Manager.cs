/**
 * The Interface for manipulating RVO2 Library in Unity
 * RVO2Manager should be attached to an empty game object in order to function properly
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVO2Manager : MonoBehaviour {
	[SerializeField] private float simulationTimeStep = 0.25f;
	// Default parameters for agents
	[Header("Agent default parameters")]
	[SerializeField] private float neighborDist = 15.0f;
	[SerializeField] private int maxNeighbors = 10;
	[SerializeField] private float timeHorizon = 10.0f;
	[SerializeField] private float timeHorizonObst = 10.0f;
	[SerializeField] private float radius = 1.5f;
	[SerializeField] private float maxSpeed = 2.0f;
	[SerializeField] private Vector2 velocity = Vector2.zero;

	private float lastUpdatedTime;

	// Initialize RVO2 Library before any other GameObject's Start() function is called
	void Awake() {
		RVO.Simulator.Instance.setTimeStep(simulationTimeStep);
		RVO.Simulator.Instance.setAgentDefaults(
			neighborDist, 
			maxNeighbors, 
			timeHorizon, 
			timeHorizonObst, 
			radius,
			maxSpeed, 
			new RVO.Vector2(velocity.x, velocity.y)
		);
	}

	// Use this for initialization
	void Start () {
		lastUpdatedTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		// Tick the simulator
		while (Time.time - lastUpdatedTime >= simulationTimeStep) {
			lastUpdatedTime += simulationTimeStep;
			RVO.Simulator.Instance.doStep();
		}
	}

	// Utility static functions
	public static RVO.Vector2 ToRVO2Vec2(Vector2 vec) {
		return new RVO.Vector2(vec.x, vec.y);
	}

	public static Vector2 ToVec2(RVO.Vector2 vec) {
		return new Vector2(vec.x(), vec.y());
	}
}

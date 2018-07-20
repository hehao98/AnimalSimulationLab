using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAgentCircle : MonoBehaviour {

	public GameObject agent;
	public float spawnRadius = 20;
	public int agentNumber = 20;

	private RVO2Agent[] agents;
	private Vector2[] goals;

	// Use this for initialization
	void Start () {
		agents = new RVO2Agent[agentNumber];
		goals  = new Vector2[agentNumber];
		for (int i = 0; i < agentNumber; ++i) {
			// Initialize agents
			agents[i] = Instantiate(agent).GetComponent<RVO2Agent>();
			agents[i].Position = new Vector2(
				spawnRadius * Mathf.Cos(i * 2 * Mathf.PI / agentNumber),
				spawnRadius * Mathf.Sin(i * 2 * Mathf.PI / agentNumber)
			);

			// Initialize goals
			goals[i] = -agents[i].Position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Set preferred velocity according to their goals
		for (int i = 0; i < agentNumber; ++i) {
			Vector2 target = goals[i] - agents[i].Position;
			agents[i].PrefVelocity = Mathf.Min(target.magnitude, agents[i].MaxSpeed) * target.normalized;
		}
	}
}

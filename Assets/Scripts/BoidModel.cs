using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidModel : MonoBehaviour {

	public int agentNumber = 100;
	public GameObject agentPrefab;
	public Vector2 spawnRangeX = new Vector2(-50, 50);
	public Vector2 spawnRangeY = new Vector2(-50, 50);
	public float seperationDistance = 3.0f;
	public float alignmentDistance = 10.0f;
	public float cohesionDistance = 20.0f;
	public float preceptionRadius = 20.0f;
	public float fieldOfViewAngle = 120.0f;
	public float noiseStrength = 2.0f;
	public float noiseInterval = 1.0f;

	private RVO2Agent[] agents;

	private float lastNoiseUpdatedTime = 0.0f;

	// Use this for initialization
	void Start () {
		agents = new RVO2Agent[agentNumber];
		for (int i = 0; i < agentNumber; ++i) {
			// Randomly generate postion and rotation
			Vector3 position = new Vector3(
				Random.Range(spawnRangeX.x, spawnRangeX.y), 
				0, 
				Random.Range(spawnRangeY.x, spawnRangeY.y)
				);
			Vector3 look = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
			if (look == Vector3.zero) look.z = 1;
			Quaternion rotation = Quaternion.LookRotation(look);	

			GameObject obj = GameObject.Instantiate(agentPrefab, position, rotation, transform);
			agents[i] = obj.GetComponent<RVO2Agent>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		// For each agent, calculate its preferred velocity
		for (int i = 0; i < agentNumber; ++i) {
			Vector2 velocity = Vector2.zero;
			
			// Add noise first
			if (Time.time - lastNoiseUpdatedTime >= noiseInterval) {
				float angle = Random.Range(0, 2 * Mathf.PI);
				velocity.x = noiseStrength * Mathf.Cos(angle);
				velocity.y = noiseStrength * Mathf.Sin(angle);
				lastNoiseUpdatedTime = Time.time;
			}

			// Calculate force from neighbors
			RVO2Agent[] neighbors = GetPerceivedNeighbors(agents[i]);
			velocity += Cohesion(agents[i], neighbors);
			velocity += Alignment(agents[i], neighbors);
			velocity += Seperation(agents[i], neighbors);

			agents[i].PrefVelocity = velocity;
		}
	}

	RVO2Agent[] GetPerceivedNeighbors(RVO2Agent agent) {
		List<RVO2Agent> list = new List<RVO2Agent>();
		for (int i = 0; i < agentNumber; ++i) {
			if (agents[i].AgentId == agent.AgentId) continue;
			float dist = (agent.Position - agents[i].Position).magnitude;
			float angle = Vector3.Angle(agent.transform.forward, agents[i].transform.forward);
			if (dist <= preceptionRadius && angle < fieldOfViewAngle) {
				list.Add(agents[i]);
			}
		}
		return list.ToArray();
	}

	Vector2 Cohesion(RVO2Agent agent, RVO2Agent[] neighbors) {
		Vector2 cohesion = Vector2.zero;
		int count = 0;

		foreach (RVO2Agent neighbor in neighbors) {
			float dist = (agent.Position - neighbor.Position).magnitude;
			if (dist <= cohesionDistance && dist >= alignmentDistance) {
				cohesion += (neighbor.Position - agent.Position).normalized;
				count++;
			}
		}

		if (count > 0) cohesion /= count;
		return cohesion;
	}
	
	Vector2 Alignment(RVO2Agent agent, RVO2Agent[] neighbors) {
		Vector2 alignment = Vector2.zero;
		int count = 0;

		foreach (RVO2Agent neighbor in neighbors) {
			float dist = (agent.Position - neighbor.Position).magnitude;
			if (dist <= alignmentDistance && dist >= seperationDistance) {
				alignment += neighbor.Velocity;
				count++;
			}
		}
		
		if (count > 0) alignment /= count;
		return alignment;
	}

	Vector2 Seperation(RVO2Agent agent, RVO2Agent[] neighbors) {
		Vector2 seperation = Vector2.zero;
		int count = 0;

		foreach (RVO2Agent neighbor in neighbors) {
			float dist = (agent.Position - neighbor.Position).magnitude;
			if (dist <= seperationDistance) {
				seperation += (agent.Position - neighbor.Position).normalized;
				count++;
			}
		}

		if (count > 0) seperation /= count;
		return seperation;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVO2Agent : MonoBehaviour {
	
	// The Id used in the RVO2 simulation system
	private int agentId;

	// Default parameters for agents
	[Header("Agent default parameters")]
	[SerializeField] private float neighborDist = 15.0f;
	public float NeighborDist { get; set; }
	[SerializeField] private int maxNeighbors = 10;
	public int MaxNeighbors { get; set; }
	[SerializeField] private float timeHorizon = 10.0f;
	public float TimeHorizon { get; set; }
	[SerializeField] private float timeHorizonObst = 10.0f;
	public float TimeHorizonObst { get; set; }
	[SerializeField] private float radius = 1.5f;
	public float Radius { get; set; } 
	[SerializeField] private float maxSpeed = 2.0f;
	public float MaxSpeed { get; set; }
	[SerializeField] private Vector2 initialVelocity = Vector2.zero;
	public Vector2 InitialVelocity { get; set; }
	[SerializeField] private Vector2 prefVelocity = Vector2.zero;
	public Vector2 PrefVelocity { get; set; }

	// Use this for initialization
	void Start () {
		agentId = RVO.Simulator.Instance.addAgent(
			RVO2Manager.ToRVO2Vec2(new Vector2(transform.position.x, transform.position.z)),
			neighborDist,
			maxNeighbors,
			timeHorizon,
			timeHorizonObst,
			radius,
			maxSpeed,
			new RVO.Vector2(initialVelocity.x, initialVelocity.y)
			);
		RVO.Simulator.Instance.setAgentPrefVelocity(agentId, new RVO.Vector2(initialVelocity.x, initialVelocity.y));
	}
	
	// Update is called once per frame
	void Update () {
		// Update preferred velocity of this agent
		RVO.Simulator.Instance.setAgentPrefVelocity(agentId, new RVO.Vector2(prefVelocity.x, prefVelocity.y));

		// Update position, note that y is the up vector in Unity
		RVO.Vector2 rvoPosition = RVO.Simulator.Instance.getAgentPosition(agentId);
		Debug.Log(rvoPosition.x() + " " + rvoPosition.y());
		transform.position = new Vector3(rvoPosition.x(), transform.position.y, rvoPosition.y());
	}
}

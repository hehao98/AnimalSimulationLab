using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CaribouRVO2AgentController : MonoBehaviour {
	DeerCharacter deerCharacter;
	RVO2Agent agent;
	
	void Start () {
		deerCharacter = GetComponent<DeerCharacter>();
		agent = GetComponent<RVO2Agent>();
		agent.updatePosition = false;
	}
	
	void Update () {	
		Vector3 velocity = new Vector3(agent.Velocity.x, 0, agent.Velocity.y);
		float forwardAmount = Vector3.Dot(velocity, transform.forward);
		float turnAmount    = Mathf.Abs(Vector3.Dot(velocity.normalized, transform.right));
		deerCharacter.forwardSpeed = forwardAmount / 7.5f;
		deerCharacter.turnSpeed    = turnAmount;
	}

	void OnAnimatorMove() {
		RVO.Vector2 rvoPosition = RVO.Simulator.Instance.getAgentPosition(agent.AgentId);
        Vector2 velocity = agent.Velocity;
        Vector3 v3d = new Vector3(velocity.x, 0, velocity.y);
		transform.position = new Vector3(rvoPosition.x(), transform.position.y, rvoPosition.y());
        transform.position += v3d * (Time.time - RVO2Manager.Instance.LastUpdatedTime);
	}
}

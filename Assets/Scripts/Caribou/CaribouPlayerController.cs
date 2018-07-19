using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CaribouPlayerController : MonoBehaviour {
	DeerCharacter deerCharacter;
	NavMeshAgent agent;
	
	void Start () {
		deerCharacter = GetComponent<DeerCharacter>();
		agent = GetComponent<NavMeshAgent>();
		agent.updatePosition = false;
		agent.updateRotation = false;
	}
	
	void Update () {	
		if (Input.GetButtonDown ("Fire1")) {
			deerCharacter.Attack();
		}
		if (Input.GetButtonDown ("Jump")) {
			deerCharacter.Jump();
		}
		if (Input.GetKeyDown (KeyCode.H)) {
			deerCharacter.Hit();
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			deerCharacter.EatStart();
		}
		if (Input.GetKeyUp (KeyCode.E)) {
			deerCharacter.EatEnd();
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			deerCharacter.SideStepL(true);
		}
		if (Input.GetKeyUp (KeyCode.F)) {
			deerCharacter.SideStepL(false);
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			deerCharacter.SideStepR(true);
		}
		if (Input.GetKeyUp (KeyCode.G)) {
			deerCharacter.SideStepR(false);
		}
		if (Input.GetKeyDown (KeyCode.K)) {
			deerCharacter.Death();
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			deerCharacter.Rebirth();
		}		
		if (Input.GetKeyDown (KeyCode.R)) {
			deerCharacter.Roar();
		}		
		if (Input.GetKeyDown (KeyCode.J)) {
			deerCharacter.SitDown();
		}		
		if (Input.GetKeyDown (KeyCode.U)) {
			deerCharacter.WakeUp();
		}	
		if (Input.GetKeyDown (KeyCode.I)) {
			deerCharacter.StandUp();
		}		
		if (Input.GetKeyDown (KeyCode.M)) {
			deerCharacter.Sleep();
		}	
		if (Input.GetKeyDown (KeyCode.N)) {
			deerCharacter.NeckControll(true);
		}		
		if (Input.GetKeyUp (KeyCode.N)) {
			deerCharacter.NeckControll(false);
		}	

		float forwardAmount = Input.GetAxis("Vertical");
		float turnAmount    = Input.GetAxis("Horizontal");
		if (Input.GetKey(KeyCode.LeftShift)) {
			forwardAmount *= 0.5f;
			turnAmount    *= 0.5f;
		}	
		deerCharacter.forwardSpeed = forwardAmount;
		deerCharacter.turnSpeed    = turnAmount;

		// Update NavMeshAgent status manually
		agent.velocity = GetComponent<Animator>().velocity;
	}
}

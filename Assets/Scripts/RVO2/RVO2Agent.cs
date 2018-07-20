using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVO2Agent : MonoBehaviour {

    public Vector2 Position { // Position in 2D
        get { return new Vector2(transform.position.x, transform.position.z); } 
        set { 
            transform.position = new Vector3(value.x, transform.position.y, value.y); 
            RVO.Simulator.Instance.setAgentPosition(agentId, new RVO.Vector2(value.x, value.y)); 
        } 
    }
    public float NeighborDist {
        get { return neighborDist; }
        set { neighborDist = value; RVO.Simulator.Instance.setAgentNeighborDist(agentId, value); }
    }
    public int MaxNeighbors {
        get { return maxNeighbors; }
        set { maxNeighbors = value; RVO.Simulator.Instance.setAgentMaxNeighbors(agentId, value); }
    }
    public float TimeHorizon {
        get { return timeHorizon; }
        set { timeHorizon = value; RVO.Simulator.Instance.setAgentTimeHorizon(agentId, value); }
    }
    public float TimeHorizonObst {
        get { return timeHorizonObst; }
        set { timeHorizonObst = value; RVO.Simulator.Instance.setAgentTimeHorizonObst(agentId, value); }
    }
    public float Radius {
        get { return radius; }
        set { radius = value; RVO.Simulator.Instance.setAgentRadius(agentId, radius); }
    }
    public float MaxSpeed {
        get { return maxSpeed; }
        set { maxSpeed = value; RVO.Simulator.Instance.setAgentMaxSpeed(agentId, value); }
    }
    public float RotateSpeed {
        get { return rotateSpeed; }
        set { rotateSpeed = Mathf.Min(value, 0); }
    }
    public Vector2 Velocity {
        get { 
            RVO.Vector2 velocity = RVO.Simulator.Instance.getAgentVelocity(agentId); 
            return new Vector2(velocity.x(), velocity.y());
        }
        set { // Useful for player control of some agent
            RVO.Simulator.Instance.setAgentVelocity(agentId, new RVO.Vector2(value.x, value.y));
        }
    }
    public Vector2 PrefVelocity {
        get { return prefVelocity; }
        set {
            prefVelocity = value;
            RVO.Simulator.Instance.setAgentPrefVelocity(agentId, new RVO.Vector2(prefVelocity.x, prefVelocity.y));
        }
    }

    // The Id used in the RVO2 simulation system
    private int agentId;
    private RVO2Manager manager;
    // Default parameters for agents
    [Header("Agent default parameters")]
    [SerializeField] private float neighborDist = 15.0f;
    [SerializeField] private int maxNeighbors = 10;
    [SerializeField] private float timeHorizon = 10.0f;
    [SerializeField] private float timeHorizonObst = 10.0f;
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private float maxSpeed = 2.0f;
    [SerializeField] private float rotateSpeed = 5.0f;
    [SerializeField] private Vector2 initialVelocity = Vector2.zero;
    [SerializeField] private Vector2 prefVelocity = Vector2.zero;

    void Awake () {
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
        RVO.Simulator.Instance.setAgentPrefVelocity(agentId, new RVO.Vector2(prefVelocity.x, prefVelocity.y));
    }

    void Start() {
        manager = RVO2Manager.Instance;
    }
    
    // Update is called once per frame
    void Update () {
        // Update position, note that y is the up vector in Unity
        RVO.Vector2 rvoPosition = RVO.Simulator.Instance.getAgentPosition(agentId);
        RVO.Vector2 velocity = RVO.Simulator.Instance.getAgentVelocity(agentId);
        Vector3 v3d = new Vector3(velocity.x(), 0, velocity.y());
        transform.position = new Vector3(rvoPosition.x(), transform.position.y, rvoPosition.y());
        transform.position += v3d * (Time.time - manager.LastUpdatedTime);

        // Update orientation
        Quaternion target = Quaternion.LookRotation(v3d);
        transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * rotateSpeed);
       //transform.rotation = target;
    }

    void OnDrawGizmos() {
        Color originalColor = Gizmos.color;

        // Draw velocity
        Gizmos.color = Color.red;
        Vector3 start = new Vector3(Position.x, transform.position.y, Position.y);
        Vector3 v3d = new Vector3(Velocity.x, 0, Velocity.y);
        Gizmos.DrawLine(start, start + v3d);
        Gizmos.DrawCube(start + v3d, Vector3.one * 0.1f);

        // Draw prefVelocity
        Gizmos.color = Color.green;
        Vector3 pv3d = new Vector3(PrefVelocity.x, 0, PrefVelocity.y);
        Gizmos.DrawLine(start, start + pv3d);
        Gizmos.DrawCube(start + pv3d, Vector3.one * 0.1f);

        Gizmos.color = originalColor;
    }
}

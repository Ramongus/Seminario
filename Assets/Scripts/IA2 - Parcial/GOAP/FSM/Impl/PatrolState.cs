using System;
using System.Runtime.InteropServices;
using FSM;
using UnityEditor.UIElements;
using UnityEngine;
using System.Linq;
//IA2-P2
public class PatrolState : MonoBaseState {

    private Player _player;
    private GOAPEnemy owner;
	[SerializeField] float sightRange;
	[SerializeField] LayerMask obstacleLayer;

	[SerializeField] float movSpeed;
	[SerializeField] Transform[] waypoints;
    [SerializeField] Transform[] waypointsReplan;
    [SerializeField] bool bounce;
	[SerializeField] float range;



	int nextWaypoint = 0;
	bool isGoingBack;

	private void Awake() {
        _player = FindObjectOfType<Player>();
        owner = GetComponent<GOAPEnemy>();
    }
    
    public override void UpdateLoop() {
		Debug.LogWarning("PATROL STATE");
        NewWaypointsReplan();
        Patrol();
    }

    public override IState ProcessInput() {
		/*var playerPosAtMyHeight = new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
		var delta = (playerPosAtMyHeight - transform.position);

		var sqrDistance = delta.sqrMagnitude;
		var dir = delta.normalized;
		var dist = delta.magnitude;

		bool isOnSight = true;
		RaycastHit hit;
		Physics.Raycast(transform.position, dir, out hit, dist, obstacleLayer);
		if (hit.collider != null)
		{
			isOnSight = false;
		}

		if (sqrDistance < sightRange * sightRange && isOnSight) {
			Debug.Log("TO CHASE STATE");
            return Transitions["OnChaseState"];
        }*/

        return this;
    }

    private void NewWaypointsReplan()
    {
        if (owner.justReplaned)
        {
            //IA2-P3
            var newWayPoints = waypoints.Concat(waypointsReplan);
            waypoints = newWayPoints.ToArray();
            owner.justReplaned = false;
        }
    }

	private void Patrol()
	{
		Vector3 toNextWaypoint = waypoints[nextWaypoint].position - transform.position;

		if (HasArriveNextWaypoint(toNextWaypoint))
			SetNextWaypoint();

		MoveToNextWaypoint(toNextWaypoint);
	}

	private bool HasArriveNextWaypoint(Vector3 toNextWaypoint)
	{
		float distanceToNextWaypoint = toNextWaypoint.magnitude;
		return distanceToNextWaypoint <= range;
	}

	private void SetNextWaypoint()
	{
		if (isGoingBack) nextWaypoint--;
		else nextWaypoint++;

		if (nextWaypoint >= waypoints.Length)
		{
			if (bounce)
			{
				isGoingBack = true;
				nextWaypoint = waypoints.Length - 2;
			}
			else
			{
				nextWaypoint = 0;
			}
		}
		else if (nextWaypoint < 0)
		{
			isGoingBack = false;
			nextWaypoint = 1;
		}
	}

	private void MoveToNextWaypoint(Vector3 toNextWaypoint)
	{
		Vector3 dir = toNextWaypoint.normalized;
		transform.forward = Vector3.Lerp(transform.forward,dir, owner.rotSpeed * Time.deltaTime);
		transform.position += transform.forward * movSpeed * Time.deltaTime;
        var t = transform.Find("ENEMYMELE");
        t.GetComponent<Animator>().SetFloat("Speed", 1);
	}
}

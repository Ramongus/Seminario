using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPatrolEnemy : StaticEnemy
{
	[SerializeField] float movSpeed;
	[SerializeField] Transform[] waypoints;
	[SerializeField] bool bounce;
	[SerializeField] float range;

	int nextWaypoint = 0;
	bool isGoingBack;

	private void Awake()
	{
		if (waypoints.Length < 2) Debug.LogError("Waypoints list has to be at least of 2 waypoints!, in: " + this.name);
	}

	void Update()
    {
		Patrol();
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

		if(nextWaypoint >= waypoints.Length)
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
		else if(nextWaypoint < 0)
		{
			isGoingBack = false;
			nextWaypoint = 1;
		}
	}

	private void MoveToNextWaypoint(Vector3 toNextWaypoint)
	{
		Vector3 dir = toNextWaypoint.normalized;
		transform.position += dir * movSpeed * Time.deltaTime;
	}
}

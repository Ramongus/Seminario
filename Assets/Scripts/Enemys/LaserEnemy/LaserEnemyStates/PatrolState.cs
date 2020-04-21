using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
	Transform owner;
	Vector3 currentDir;
	int index;
	float speed;
	bool loop;
	bool goingBack;
	List<Transform> waypoints;

	public PatrolState(StateMachine sm, Transform owner, float speed, List<Transform> waypoints, bool loop) : base(sm)
	{
		this.waypoints = new List<Transform>(waypoints);
		index = 0;
		this.speed = speed;
		this.owner = owner;
		this.loop = loop;
		goingBack = false;
	}

	public override void Execute()
	{
		int indexOfPrevious = SetValidPreviousIndex();
		CheckChangeOfDirection(indexOfPrevious);
		MoveToNextWaypoint();
	}

	private void MoveToNextWaypoint()
	{
		Vector3 currentDir = GetDirToWaypoint(waypoints[index]);
		owner.forward = currentDir;
		owner.position += owner.forward * speed * Time.deltaTime;
	}

	private void CheckChangeOfDirection(int indexOfPrevious)
	{
		float distancePreviousNext = GetDistanceOfWaypoints(waypoints[indexOfPrevious], waypoints[index]);
		float distancePreviousToOwner = GetDistanceFromPreviousToOwner(waypoints[indexOfPrevious]);
		if (distancePreviousToOwner >= distancePreviousNext)
			SetNextWaypointIndex();
	}

	private void SetNextWaypointIndex()
	{
		if (loop)
			SetLoopNextWaypointIndex();
		else
			SetBounceNextWaypointIndex();
	}


	private void SetLoopNextWaypointIndex()
	{
		index++;
		if (index >= waypoints.Count)
			index = 0;
	}

	private void SetBounceNextWaypointIndex()
	{
		if (goingBack)
			index--;
		else
			index++;
		if(index >= waypoints.Count)
		{
			goingBack = true;
			index--;
		}
		else if(index < 0)
		{
			goingBack = false;
			index = 1;
		}
	}

	private float GetDistanceFromPreviousToOwner(Transform previous)
	{
		return (owner.position - previous.position).magnitude;
	}

	private float GetDistanceOfWaypoints(Transform w1, Transform w2)
	{
		return (w1.position - w2.position).magnitude;
	}

	private int SetValidPreviousIndex()
	{
		int previousIndex;
		if (loop)
			previousIndex = GetPreviousIndexOfLooping();
		else
			previousIndex = GetPreviousIndezOfBounce();
		return previousIndex;
	}

	private int GetPreviousIndexOfLooping()
	{
		if (index - 1 < 0)
			return waypoints.Count - 1;
		return index - 1;
	}

	private int GetPreviousIndezOfBounce()
	{
		if (goingBack)
			if (index + 1 >= waypoints.Count)
				return waypoints.Count - 1;
			else
				return index + 1;
		else
			return GetPreviousIndexOfLooping();
	}

	private Vector3 GetDirToWaypoint(Transform waypoint)
	{
		return (waypoint.position - owner.position).normalized;
	}
}

using UnityEngine;

public class WaypointsUtilities
{
	public bool HasToChangeDirection(Transform previousWaypoint, Transform nextWaypoint, Transform entity)
	{
		float distancePreviousNext = GetDistanceOfWaypoints(previousWaypoint, nextWaypoint);
		float distancePreviousToOwner = GetDistanceFromPreviousToOwner(previousWaypoint, entity);
		if (distancePreviousToOwner >= distancePreviousNext)
			return true;
		return false;
	}

	public void MoveToNextWaypoint(Transform entity, Transform nextWaypoint, float speed)
	{
		Vector3 currentDir = GetDirToWaypoint(nextWaypoint, entity);
		entity.forward = currentDir;
		entity.position += entity.forward * speed * Time.deltaTime;
	}

	private Vector3 GetDirToWaypoint(Transform waypoint, Transform entity)
	{
		return (waypoint.position - entity.position).normalized;
	}

	public float GetDistanceFromPreviousToOwner(Transform previousWaypoint, Transform entity)
	{
		return Vector3.Distance(previousWaypoint.position, entity.position);
	}

	public float GetDistanceOfWaypoints(Transform w1, Transform w2)
	{
		return Vector3.Distance(w1.position, w2.position);
	}
}

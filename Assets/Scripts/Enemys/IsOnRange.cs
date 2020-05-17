using UnityEngine;

public class IsOnRange
{
	public bool Execute(Vector3 owner, Vector3 target, float range)
	{
		return Vector3.Distance(owner, target) <= range;
	}
}

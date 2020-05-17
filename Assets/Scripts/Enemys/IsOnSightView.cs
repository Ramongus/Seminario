using UnityEngine;

public class IsOnSightView
{
	public bool Execute(Vector3 ownerPosition, Vector3 ownerSightDir, Vector3 objectivePosition, float angleOfSight)
	{
		Vector3 dirToObjective = Vector3.Normalize(objectivePosition - ownerPosition);
		Vector3 ignoreHeight = new Vector3(dirToObjective.x, 0, dirToObjective.z).normalized;
		return Vector3.Angle(ownerSightDir, ignoreHeight) <= angleOfSight;
	}
}

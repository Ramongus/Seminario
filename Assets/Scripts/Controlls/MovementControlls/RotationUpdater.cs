using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationUpdater
{
	Transform aimPointer;
	Transform playerTransform;

	public RotationUpdater(Transform aimPointer, Transform playerT)
	{
		this.aimPointer = aimPointer;
		playerTransform = playerT;
	}

	public void UpdateRotation()
	{
		Vector3 dir = Vector3.Normalize(aimPointer.position - playerTransform.position);
		Vector3 finalDir = new Vector3(dir.x, 0, dir.z);
		playerTransform.forward = finalDir;
	}
}

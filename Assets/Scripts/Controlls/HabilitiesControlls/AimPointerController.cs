using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPointerController
{
	[SerializeField] float sensitivity;
	[SerializeField] Transform aimPointer;

	public AimPointerController(float sensitivity, Transform aimPointer)
	{
		this.sensitivity = sensitivity;
		this.aimPointer = aimPointer;
	}

	public void AimUpdate()
	{
		Vector3 finalMove = new Vector3();

		if(Input.GetAxis("Horizontal2") != 0)
		{
			finalMove = new Vector3(Input.GetAxis("Horizontal2"), 0, finalMove.z);
		}
		if (Input.GetAxis("Vertical2") != 0)
		{
			finalMove = new Vector3(finalMove.x, 0, Input.GetAxis("Vertical2"));
		}

		aimPointer.position += finalMove * sensitivity * Time.deltaTime;
	}
}

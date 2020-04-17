using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointerController
{
	[SerializeField] float sensitivity;
	[SerializeField] Transform aimPointer;
	bool isJoystick;

	public PlayerPointerController(float sensitivity, Transform aimPointer, bool isJoystick)
	{
		this.sensitivity = sensitivity;
		this.aimPointer = aimPointer;
		this.isJoystick = isJoystick;
	}

	public void AimUpdate()
	{
		if (isJoystick)
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
		else
		{
			Ray rayMouseFromCamera = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(rayMouseFromCamera, out hit, 100f, 1<<LayerMask.NameToLayer("Arena")))
			{
				aimPointer.transform.position = hit.point;
			}
		}
	}
}

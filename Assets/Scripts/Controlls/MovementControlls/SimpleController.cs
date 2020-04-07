using UnityEngine;

public class SimpleController
{
    public Vector3 GetMovementAxis()
    {
		Vector3 axis = new Vector3();

		if(Input.GetAxis("Horizontal") != 0)
		{
			axis += Vector3.right * Input.GetAxis("Horizontal");
		}

		if (Input.GetAxis("Vertical") != 0)
		{
			axis += Vector3.forward * Input.GetAxis("Vertical");
		}

		axis = Vector3.ClampMagnitude(axis, 1);

		return axis;
	}
}

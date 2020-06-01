using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateConstantly : MonoBehaviour
{
	[SerializeField] Vector3 rotationAxis;
	[SerializeField] float rotationSpeed;

	private void Update()
	{
		transform.Rotate(rotationAxis * rotationSpeed);
	}
}

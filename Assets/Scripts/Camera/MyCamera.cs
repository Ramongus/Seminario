using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
	[SerializeField] Transform target;
	[SerializeField] float cameraSpeed;
	[SerializeField] float deadZoneRadius;

	Vector3 initialDistanceToTarget;

    // Start is called before the first frame update
    void Start()
    {
		initialDistanceToTarget = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
		//1
		//transform.position = Vector3.Lerp(transform.position, target.position + initialDistanceToTarget, Time.deltaTime * cameraSpeed);

		//2
		/*
		Vector3 finalPos = target.position + initialDistanceToTarget;
		Vector3 cameraDir = (finalPos - transform.position).normalized;

		if (!((finalPos - transform.position).magnitude < deadZoneRadius))
			transform.position += cameraDir * cameraSpeed * Time.deltaTime;
		*/
		Vector3 finalPos = target.position + initialDistanceToTarget;
		transform.position = finalPos;

	}
}

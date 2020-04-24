using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvas : MonoBehaviour
{
	[SerializeField] Transform parentPosition;
	Vector3 intialDifference;
	Quaternion initialRotation;

	private void Awake()
	{
		initialRotation = transform.rotation;
		intialDifference = transform.position - parentPosition.position;
	}

	private void Update()
	{
		transform.SetPositionAndRotation(parentPosition.position + intialDifference, initialRotation);
	}
}

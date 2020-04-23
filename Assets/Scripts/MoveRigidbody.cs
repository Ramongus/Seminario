using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRigidbody : MonoBehaviour, IMoveBehaviour
{
	[SerializeField] float speed;
	Vector3 velocity;
	Rigidbody rigi;

	private void Awake()
	{
		rigi = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		rigi.velocity = new Vector3(velocity.x * speed, rigi.velocity.y, velocity.z * speed);
	}

	public void SetVelocity(Vector3 velocity)
	{
		this.velocity = velocity;
	}
}

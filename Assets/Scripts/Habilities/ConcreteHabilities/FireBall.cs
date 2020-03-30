using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : AbstractHabilities
{
	[SerializeField] float speed;

	private void Start()
	{
		myPowers.Add(Powers.Power.Impulse);
		myPowers.Add(Powers.Power.Fire);
		powersInteractions.Add(Powers.Power.Smash, Explode);
	}

	private void Explode()
	{
		Debug.LogWarning("FireBall explode, but is not still implemented");
		Destroy(this.gameObject);
	}

	private void Update()
	{
		transform.position += speed * transform.forward * Time.deltaTime;
	}

	/*
	public void OnCollisionEnter(Collision collision)
	{
		AbstractHabilities habilitie = collision.gameObject.GetComponent<AbstractHabilities>();
		if (habilitie != null)
		{
			if (InteractWith(habilitie))
				return;
		}
	}
	*/

	private void OnTriggerEnter(Collider other)
	{
		AbstractHabilities habilitie = other.gameObject.GetComponent<AbstractHabilities>();
		if (habilitie != null)
		{
			if (InteractWith(habilitie))
				return;
		}
	}

	public override void SetInitiation(Vector3 castPos, Vector3 playerPos)
	{
		transform.position = playerPos;
		Vector3 dirIgnoreHeight = new Vector3(castPos.x - playerPos.x, 0, castPos.z - playerPos.z);
		transform.forward = dirIgnoreHeight;
	}
}

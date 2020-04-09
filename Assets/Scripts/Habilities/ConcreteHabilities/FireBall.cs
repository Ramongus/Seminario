using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : AbstractHabilities
{
	[SerializeField] float speed;
	[SerializeField] GameObject explodeParticles;

	private void Start()
	{
		myPowers.Add(Powers.Power.Impulse);
		myPowers.Add(Powers.Power.Fire);
		powersInteractions.Add(Powers.Power.Smash, Explode);
	}

	private void Explode()
	{
		Debug.LogWarning("FireBall explode, but is not still implemented");
		CreateExplodeParticles();
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

	override protected void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		Player collideWithPlayer = other.gameObject.GetComponent<Player>();
		if(collideWithPlayer != null)
		{

		}
		else
		{
			AbstractHabilities habilitie = other.gameObject.GetComponent<AbstractHabilities>();
			if (habilitie != null)
			{
				if (InteractWith(habilitie))
					return;
			}
			CreateExplodeParticles();
		}
	}

	private void CreateExplodeParticles()
	{
		GameObject particlesOnExplode = Instantiate(explodeParticles);
		particlesOnExplode.transform.position = this.transform.position;
		particlesOnExplode.GetComponent<ParticleSystem>().Play();
	}

	public override void SetInitiation(Vector3 castPos, Vector3 playerPos)
	{
		transform.position = playerPos + Vector3.up * initialHeight;
		Vector3 dirIgnoreHeight = new Vector3(castPos.x - playerPos.x, 0, castPos.z - playerPos.z);
		transform.forward = dirIgnoreHeight;
	}
}

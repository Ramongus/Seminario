using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMetheore : AbstractAbilities
{
	[SerializeField] float explodeDivisionForce;
	[SerializeField] RockMetheore rockMetheorePrefab;
	[SerializeField] GameObject fallingParticles;
	[SerializeField] GameObject impactParticles;
	[SerializeField] GameObject circleMarkerParticles;
	[SerializeField] GameObject destroyableFbx;
	[SerializeField] GameObject rockRender;

	private Collider myCollider;
	private Rigidbody rigi;
	private bool isFalling;
	private Vector3 impulseDir;
	private bool isDevided;

	protected override void Awake()
	{
		base.Awake();
		rigi = GetComponent<Rigidbody>();
		isDevided = false;
		myCollider = GetComponent<Collider>();
		isFalling = true;
		myPowers.Add(Powers.Power.Smash);
		powersInteractions.Add(Powers.Power.Impulse, ImpuseInteraction);
		powersInteractions.Add(Powers.Power.Reflec, Explode);
	}

	override protected void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 10)
		{
			isFalling = false;
		}
		impulseDir = Vector3.Normalize(this.transform.position - collision.transform.position);
		impulseDir = new Vector3(impulseDir.x, 0, impulseDir.z);
		if (isFalling)
		{
			IPlayer player = collision.gameObject.GetComponent<IPlayer>();
			if (player != null)
				DoDamage(player);
		}

		AbstractAbilities habilitie = collision.gameObject.GetComponent<AbstractAbilities>();
		if (habilitie != null)
		{
			Debug.Log("Lo toco una habilidad");
			if (InteractWith(habilitie))
				return;
		}


		if (collision.gameObject.layer == 13 || collision.gameObject.layer == 12)
		{
			Explode();
			if (isFalling)
			{
				HealthSystem hasHealSystem = collision.gameObject.GetComponent<HealthSystem>();
				if (hasHealSystem != null)
				{
					if (isHealHabilitie)
						hasHealSystem.Sethealth(hasHealSystem.GetHealth() + powerValue);
					else
						hasHealSystem.Sethealth(hasHealSystem.GetHealth() - powerValue);
				}
			}
		}

		//base.OnCollisionEnter(collision);

		if(!isDevided)
			OnImpact();
	}

	private void OnImpact()
	{
		isFalling = false;
		SetImpactParticles();
	}

	private void SetImpactParticles()
	{
		fallingParticles.SetActive(false);
		impactParticles.SetActive(true);
		//circleMarkerParticles.SetActive(true);
	}

	private void SetDividedParticles()
	{
		fallingParticles.SetActive(true);
		impactParticles.SetActive(false);
		circleMarkerParticles.SetActive(false);
	}

	override protected void OnTriggerEnter(Collider collision)
	{
		impulseDir = Vector3.Normalize(this.transform.position - collision.transform.position);
		//Debug.Log(impulseDir);
		impulseDir = new Vector3(impulseDir.x, 0, impulseDir.z);
		if (isFalling)
		{
			IPlayer player = collision.gameObject.GetComponent<IPlayer>();
			if (player != null)
				DoDamage(player);
		}

		AbstractAbilities habilitie = collision.gameObject.GetComponent<AbstractAbilities>();
		if (habilitie != null)
		{
			Debug.Log("Lo toco una habilidad");
			if (InteractWith(habilitie))
				return;
		}

		if (!isDevided)
			OnImpact();
		base.OnTriggerEnter(collision);
	}

	private void ImpuseInteraction()
	{
		if (!isDevided)
		{
			float angleChange = -30f;
			List<RockMetheore> smallRocks = new List<RockMetheore>();
			smallRocks.Add(Instantiate(rockMetheorePrefab));
			smallRocks.Add(Instantiate(rockMetheorePrefab));
			smallRocks.Add(Instantiate(rockMetheorePrefab));
			foreach (RockMetheore rock in smallRocks)
			{
				rock.isDevided = true;
				rock.SetDividedParticles();
				rock.transform.position = transform.position;
				rock.transform.localScale = transform.localScale / 3;
				rock.rigi.useGravity = false;
				rock.GetCollider().isTrigger = true;
				rock.transform.forward = impulseDir;
				rock.transform.Rotate(rock.transform.up * angleChange);
				angleChange += 30;
				rock.rigi.AddForce(rock.transform.forward * explodeDivisionForce, ForceMode.Impulse);
				rock.powerValue = this.powerValue * 2;
			}
			angleChange = -30f;
			Debug.Log("SE DESTRUYE POR IMPULSE INTERACTION");
			Explode();
			//Destroy(this.gameObject);
		}
	}

	private void Explode()
	{
		rockRender.SetActive(false);
		destroyableFbx.SetActive(true);
		destroyableFbx.GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Collider>().enabled = false;
		GetComponent<Rigidbody>().isKinematic = true;
	}

	private void DoDamage(IPlayer player)
	{
		player.SetHealth(player.GetHP() - powerValue);
	}

	private Collider GetCollider()
	{
		return myCollider;
	}

	private void SetIsDivided(bool devided)
	{
		isDevided = devided;
	}

	public override void SetInitiation(Vector3 castPos, Vector3 playerPos)
	{
		transform.position = castPos + Vector3.up * initialHeight;
	}
}
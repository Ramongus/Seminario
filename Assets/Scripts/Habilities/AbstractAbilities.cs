using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAbilities : MonoBehaviour
{
	[SerializeField] protected string habilitieName;
	[SerializeField] protected float powerValue;
	[SerializeField] protected bool isHealHabilitie;
	[SerializeField] protected float initialHeight;
	protected float lifetime;

	[SerializeField] protected float cooldown;
	protected float cooldownTimer;


	protected List<Powers.Power> myPowers;
	protected Dictionary<Powers.Power, Action> powersInteractions; 

	virtual protected void Awake()
	{
		lifetime = 10;
		myPowers = new List<Powers.Power>();
		powersInteractions = new Dictionary<Powers.Power, Action>();
		StartCoroutine(AutoDestruction());
	}

	virtual protected void Update()
	{

	}

	public Powers.Power[] GetPowers()
	{
		return myPowers.ToArray();
	}

	protected bool InteractWith(AbstractAbilities h)
	{
		foreach (Powers.Power power in h.GetPowers())
		{
			foreach (Powers.Power myPowerInteraction in powersInteractions.Keys)
			{
				if (myPowerInteraction.Equals(power))
				{
					powersInteractions[myPowerInteraction]();
					return true;
				}
			}
		}
		return false;
	}

	public string GetName()
	{
		return habilitieName;
	}

	public bool IsHabilitieName(string name)
	{
		return this.habilitieName == name;
	}

	public virtual float GetPowerValue()
	{
		return powerValue;
	}

	public bool IsHealHabilitie()
	{
		return isHealHabilitie;
	}

	IEnumerator AutoDestruction()
	{
		yield return new WaitForSeconds(lifetime);
		Destroy(this.gameObject);
		yield return null;
	}

	virtual protected void OnTriggerEnter(Collider other)
	{
		IDamageable player = other.GetComponent<IDamageable>();
		if(player != null)
		{
			if (isHealHabilitie)
			{
				player.SetHealth(player.GetHealth() + powerValue);
				Destroy(this.gameObject);
			}
		}

		if(other.gameObject.layer == 12 || other.gameObject.layer == 13)
		{
			HealthSystem hasHealSystem = other.gameObject.GetComponent<HealthSystem>();
			if (hasHealSystem != null)
			{
				if (isHealHabilitie)
					hasHealSystem.Sethealth(hasHealSystem.GetHealth() + powerValue);
				else
					hasHealSystem.Sethealth(hasHealSystem.GetHealth() - powerValue);
			}

			Destroy(gameObject);
		}
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 12 || collision.gameObject.layer == 13)
		{
			HealthSystem hasHealSystem = collision.gameObject.GetComponent<HealthSystem>();
			if (hasHealSystem != null)
			{
				if (isHealHabilitie)
					hasHealSystem.Sethealth(hasHealSystem.GetHealth() + powerValue);
				else
					hasHealSystem.Sethealth(hasHealSystem.GetHealth() - powerValue);
			}

			Debug.Log("SE DESTRUYE POR Collision INTERACTION");
			Debug.Log(collision.gameObject);
			Destroy(gameObject);
		}
	}

	public abstract void SetInitiation(Vector3 castPos, Vector3 playerPos);
}

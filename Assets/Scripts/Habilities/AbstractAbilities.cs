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


	protected List<Powers.Power> myPowers;
	protected Dictionary<Powers.Power, Action> powersInteractions; 

	virtual protected void Awake()
	{
		lifetime = 10;
		myPowers = new List<Powers.Power>();
		powersInteractions = new Dictionary<Powers.Power, Action>();
		StartCoroutine(AutoDestruction());
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
		/*
		AbstractEnemy enemy = other.gameObject.GetComponent<AbstractEnemy>();
		if (enemy)
		{
			enemy.SetHp(enemy.GetHP() - powerValue);
			Destroy(gameObject);
		}
		*/
		if(other.gameObject.layer == 12 || other.gameObject.layer == 13)
		{
			Destroy(gameObject);
		}
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 12 || collision.gameObject.layer == 13)
		{
			Destroy(gameObject);
		}
	}

	public abstract void SetInitiation(Vector3 castPos, Vector3 playerPos);
}

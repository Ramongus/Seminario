using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractHabilities : MonoBehaviour
{
	[SerializeField] protected string habilitieName;
	[SerializeField] protected float powerValue;

	protected List<Powers.Power> myPowers;
	protected Dictionary<Powers.Power, Action> powersInteractions; 

	virtual protected void Awake()
	{
		myPowers = new List<Powers.Power>();
		powersInteractions = new Dictionary<Powers.Power, Action>();
	}

	public Powers.Power[] GetPowers()
	{
		return myPowers.ToArray();
	}

	protected bool InteractWith(AbstractHabilities h)
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

	public abstract void SetInitiation(Vector3 castPos, Vector3 playerPos);
}

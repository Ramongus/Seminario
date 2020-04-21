using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : AbstractAbilities, IChargeAttack, IDoDamage
{
	[SerializeField] float timeToCharge;
	float chargeAmount;

	public bool Charge()
	{
		chargeAmount += Time.deltaTime;
		if(chargeAmount >= timeToCharge)
		{
			ResetCharge();
			return true;
		}
		return false;
	}

	public void Attack()
	{
		throw new System.NotImplementedException();
	}

	public void DoDamage(IDamageable damageable)
	{
		throw new System.NotImplementedException();
	}

	public override void SetInitiation(Vector3 castPos, Vector3 playerPos)
	{
		throw new System.NotImplementedException();
	}

	public void ResetCharge()
	{
		chargeAmount = 0;
	}
}

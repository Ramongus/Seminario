using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealArea : AbstractAbilities
{
	[SerializeField] float healCooldown;
	float cooldownTimer;

	protected override void Awake()
	{
		base.Awake();
		isHealHabilitie = true;
		myPowers.Add(Powers.Power.Heal);
		powersInteractions.Add(Powers.Power.Fire, Evaporate);
	}

	private void Evaporate()
	{
		//Instanciar particulas de evaporacion

		//Sustituir destruccion por desaparición
		Destroy(this.gameObject);
	}

	private void Update()
	{
		cooldownTimer -= Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		AbstractAbilities habilitie = other.gameObject.GetComponent<AbstractAbilities>();
		if (habilitie != null)
		{
			Debug.Log("Lo toco una habilidad");
			if (InteractWith(habilitie))
				return;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		IDamageable damageable = other.GetComponent<IDamageable>();
		if(damageable != null)
		{
			if(cooldownTimer <= 0)
			{
				cooldownTimer = healCooldown;
				damageable.SetHealth(damageable.GetHealth() + powerValue);
			}
		}
	}

	public override void SetInitiation(Vector3 castPos, Vector3 playerPos)
	{
		transform.position = castPos;
	}
}

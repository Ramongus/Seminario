using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealArea : AbstractAbilities
{
	[SerializeField] float healCooldown;
	float healTimer;

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

	override protected void Update()
	{
		base.Update();
		healTimer -= Time.deltaTime;
	}

	override protected void OnTriggerEnter(Collider other)
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
			if(healTimer <= 0)
			{
				healTimer = healCooldown;
				damageable.SetHealth(damageable.GetHealth() + powerValue);
			}
		}
	}

	public override void SetInitiation(Vector3 castPos, Vector3 playerPos)
	{
		transform.position = castPos;
	}
}

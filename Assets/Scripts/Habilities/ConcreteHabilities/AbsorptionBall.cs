using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorptionBall : AbstractHabilities
{
	[SerializeField] float speed;

	AbstractHabilities lastHabilitieContact;

	protected override void Awake()
	{
		base.Awake();
		myPowers.Add(Powers.Power.Absorption);
		myPowers.Add(Powers.Power.Impulse);
		powersInteractions.Add(Powers.Power.Fire, Absorb);
		powersInteractions.Add(Powers.Power.Heal, AbsorbHeal);
	}

	
	private void AbsorbHeal()
	{
		powerValue += lastHabilitieContact.GetPowerValue();
		transform.localScale *= 1.3f;
		GetComponent<Renderer>().material.color = Color.green;
		Debug.Log("Absoption ball has absorb de power of: " + lastHabilitieContact.name);
		Destroy(lastHabilitieContact.gameObject);
	}
	

	public void Absorb()
	{
		powerValue += lastHabilitieContact.GetPowerValue();
		transform.localScale *= 1.3f;
		ChangeMaterialToAbsorbedSpell(lastHabilitieContact);
		Debug.Log("Absoption ball has absorb de power of: " + lastHabilitieContact.name);
		Destroy(lastHabilitieContact.gameObject);
	}

	private void ChangeMaterialToAbsorbedSpell(AbstractHabilities abilitie)
	{
		GetComponent<Renderer>().material = abilitie.GetComponent<Renderer>().material;
	}

	private void Update()
	{
		transform.position += transform.forward * speed * Time.deltaTime;
	}

	public override void SetInitiation(Vector3 castPos, Vector3 playerPos)
	{
		transform.position = playerPos + Vector3.up * initialHeight;
		Vector3 dirIgnoreHeight = new Vector3(castPos.x - playerPos.x, 0, castPos.z - playerPos.z);
		transform.forward = dirIgnoreHeight;
	}

	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Absorption Ball Detect Collision");
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Absorption Ball Detect Trigger");
		AbstractHabilities habilitie = other.gameObject.GetComponent<AbstractHabilities>();
		if (habilitie != null)
		{
			lastHabilitieContact = habilitie;
			if (InteractWith(habilitie))
				return;
		}
	}
}

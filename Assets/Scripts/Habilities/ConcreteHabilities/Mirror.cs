using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : AbstractAbilities
{
	AbstractAbilities lastHabilitieContact;

	protected override void Awake()
	{
		base.Awake();
		myPowers.Add(Powers.Power.Reflec);
		powersInteractions.Add(Powers.Power.Impulse, ReflectHabilitie);
	}

	private void ReflectHabilitie()
	{
		Vector3 habilitieOpositeDir = -lastHabilitieContact.transform.forward;
		float angleWithZAxis = Vector3.Angle(habilitieOpositeDir, transform.forward);
		float angleWithXAxis = Vector3.Angle(habilitieOpositeDir, transform.right);
		lastHabilitieContact.transform.forward = transform.forward;
		if (angleWithXAxis < 90)
		{
			lastHabilitieContact.transform.Rotate(Vector3.up * -angleWithZAxis);
		}
		else
		{
			lastHabilitieContact.transform.Rotate(Vector3.up * angleWithZAxis);
		}
	}

	public override void SetInitiation(Vector3 castPos, Vector3 playerPos)
	{
		Vector3 dirIgnoringHeighDif = new Vector3(castPos.x - playerPos.x, 0, castPos.z - playerPos.z);
		transform.forward = dirIgnoringHeighDif.normalized;
		transform.position = castPos;
	}

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
	}

	protected override void OnTriggerEnter(Collider other)
	{
		Debug.Log("Mirror Detect Trigger");
		AbstractAbilities habilitie = other.gameObject.GetComponent<AbstractAbilities>();
		if (habilitie != null)
		{
			lastHabilitieContact = habilitie;
			if (InteractWith(habilitie))
				return;
		}
		base.OnTriggerEnter(other);
	}
}

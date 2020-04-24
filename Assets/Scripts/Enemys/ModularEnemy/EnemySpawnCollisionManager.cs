using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnCollisionManager : MonoBehaviour
{
	HealthSystem healthSystem;

	private void Awake()
	{
		healthSystem = GetComponent<HealthSystem>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Esta collisionando");
		AbstractAbilities abilitie = collision.collider.GetComponent<AbstractAbilities>();
		if(abilitie != null)
		{
			if(abilitie.IsHealHabilitie())
				healthSystem.Sethealth(healthSystem.GetHealth() + abilitie.GetPowerValue());
			else
				healthSystem.Sethealth(healthSystem.GetHealth() - abilitie.GetPowerValue());
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Esta trigeriando");
		AbstractAbilities abilitie = other.GetComponent<AbstractAbilities>();
		if (abilitie != null)
		{
			Debug.Log("Esta triggeriando con una habilidad");
			/*
			if (abilitie.IsHealHabilitie())
				healthSystem.Sethealth(healthSystem.GetHealth() + abilitie.GetPowerValue());
			else
				healthSystem.Sethealth(healthSystem.GetHealth() - abilitie.GetPowerValue());
			*/
		}
	}
}

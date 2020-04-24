using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
	HealthSystem healthSystem;
	float health;
	public event EventHandler OnEnemyDies;

	private void Awake()
	{
		healthSystem = GetComponent<HealthSystem>() ?? throw new MissingComponentException("TE FALTA AGREGARLE UN SISTEMA DE SALUD, BOLUDO!");
		healthSystem.OnHealthChange += HealthSystem_OnHealthChange;
	}

	private void HealthSystem_OnHealthChange(object sender, EventArgs e)
	{
		health = healthSystem.GetHealth();
		if (!IsAlive())
			Die();
	}

	private void Die()
	{
		OnEnemyDies?.Invoke(this, EventArgs.Empty);
	}

	public bool IsAlive()
	{
		return health > 0;
	}

	public void SpawnEnemy()
	{
		gameObject.SetActive(true);
	}
}

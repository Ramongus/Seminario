using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
	public bool die;
	[SerializeField] float health;
	public event EventHandler OnEnemyDies;

	private void Update()
	{
		/*
		 * This update is for Dies event Test
		 */
		if (die)
		{
			health = 0;
			Die();
			die = false;
		}
	}

	public void Die()
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

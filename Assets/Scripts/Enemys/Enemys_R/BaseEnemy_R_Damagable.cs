using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy_R_Damagable : GridEntity, IDamageable
{
	[Header("Damageable Interface")]
	[SerializeField] float maxHealth;
	float health;

	protected override void Awake()
	{
		base.Awake();
		health = maxHealth;
	}

	public float GetHealth()
	{
		return health;
	}

	public void SetHealth(float health)
	{
		this.health = health;
		if (health <= 0)
			Die();
	}

	protected virtual void Die()
	{
		Destroy(this.gameObject);
	}
}

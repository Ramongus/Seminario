using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnHealthSystem : HealthSystem
{
	[SerializeField] float maxHp;
	float health;

	[SerializeField] Image healthBar;

	[SerializeField] bool hasDestroyableFBX;
	[SerializeField] GameObject destroyableFBX;
	[SerializeField] GameObject render;

	[SerializeField] GameObject healthBarContainer;

	public override event EventHandler OnHealthChange;

	private void Awake()
	{
		health = maxHp;
		UpdateHealthBar();
	}

	public override float GetHealth()
	{
		return health;
	}

	public override void Sethealth(float health)
	{
		if (health >= maxHp)
			health = maxHp;
		else if (health <= 0)
			Die();

		this.health = health;
		UpdateHealthBar();
		OnHealthChange?.Invoke(this, EventArgs.Empty);
	}

	private void Die()
	{
		health = 0;
		GetComponent<Animator>()?.SetTrigger("Die");
		if (hasDestroyableFBX)
		{
			render.SetActive(false);
			destroyableFBX.SetActive(true);
		}
		GetComponent<Rigidbody>().isKinematic = true;
		Collider[] allColliders = GetComponents<Collider>();
		for (int i = 0; i < allColliders.Length; i++)
		{
			allColliders[i].enabled = false;
		}
		healthBarContainer.SetActive(false);

		MonoBehaviour[] components = GetComponents<MonoBehaviour>();
		foreach (MonoBehaviour monoBehaviour in components)
		{
			monoBehaviour.enabled = false;
		}
	}

	private void UpdateHealthBar()
	{
		healthBar.fillAmount = health / maxHp;
	}
}

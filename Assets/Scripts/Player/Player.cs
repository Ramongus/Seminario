using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
	[SerializeField] float maxHP;
	float currentHP;

	[SerializeField] List<AbstractHabilities> myHabilities;

	[SerializeField] float aimSensitivity;
	[SerializeField] Transform aimPointer;

	[SerializeField] float movementSpeed;

	[SerializeField] bool isJoystickPlayer;

	PlayerLogic logic;

	HabilitiesManager myHabilitiesManager;

	Animator animator;

	public Player(float maxHP)
	{
		this.maxHP = maxHP;
		currentHP = maxHP;
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
		logic = new PlayerLogic(transform, movementSpeed, aimPointer, aimSensitivity, animator, isJoystickPlayer);
		myHabilitiesManager = new HabilitiesManager(myHabilities, this);
		currentHP = maxHP;
		StartCoroutine(DebugHealth());
	}

	private void Update()
	{
		logic.Logic();
	}

	public void SetHealth(float health)
	{
		if (health >= maxHP)
		{
			currentHP = maxHP;
			return;
		}
		else if (health <= 0)
		{
			currentHP = 0;
			return;
		}
		DamageOrHealAnimation(health);
		currentHP = health;
	}

	private void DamageOrHealAnimation(float health)
	{
		if (health >= currentHP)
		{
			HealthAnimation();
		}
		else
		{
			DamagedAnimation();
		}
	}

	private void HealthAnimation()
	{
		Debug.LogWarning("Heal animation its not already implemented");
	}

	private void DamagedAnimation()
	{
		Debug.LogWarning("Damaged animation its not already implemented");
	}

	public float GetHP()
	{
		return currentHP;
	}

	public float GetMaxHP()
	{
		return maxHP;
	}

	IEnumerator DebugHealth()
	{
		while (true)
		{
			Debug.Log("My health is: " + currentHP);
			yield return new WaitForSeconds(5);
		}
	}
}
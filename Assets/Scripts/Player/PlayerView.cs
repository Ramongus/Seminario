using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour, IPlayer
{
	[SerializeField] Image healthBar;
	[SerializeField] float maxHP;
	float currentHP;

	[SerializeField] List<AbstractHabilities> myHabilities;

	[SerializeField] float aimSensitivity;
	[SerializeField] Transform aimPointer;

	[SerializeField] float movementSpeed;

	[SerializeField] bool isJoystickPlayer;

	PlayerModel logic;

	HabilitiesManager myHabilitiesManager;

	Animator animator;

	public PlayerView(float maxHP)
	{
		this.maxHP = maxHP;
		currentHP = maxHP;
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
		logic = new PlayerModel(transform, movementSpeed, aimPointer, aimSensitivity, animator, isJoystickPlayer);
		myHabilitiesManager = new HabilitiesManager(myHabilities, this);
		currentHP = maxHP;
		StartCoroutine(DebugHealth());
		EventsManager.SuscribeToEvent("Dash", Dash);
	}

	private void Update()
	{
		logic.Logic();
	}

	//
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	public void SetHealth(float health)
	{
		if (health >= maxHP)
			currentHP = maxHP;
		else if (health <= 0)
			currentHP = 0;
		else
			currentHP = health;

		DamageOrHealAnimation(health);
		healthBar.fillAmount = currentHP / maxHP;
	}
	//
	//
	//
	//


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

	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	public float GetHP()
	{
		return currentHP;
	}

	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
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

	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	public void Dash(params object[] parameters)
	{

	}
}
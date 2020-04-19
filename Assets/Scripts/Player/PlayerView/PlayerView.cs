using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour, IPlayer
{
	[SerializeField] Image healthBar;
	[SerializeField] float maxHP;

	[SerializeField] List<AbstractHabilities> myHabilities;

	[SerializeField] float aimSensitivity;
	[SerializeField] Transform aimPointer;

	[SerializeField] float movementSpeed;

	HabilitiesManager myHabilitiesManager;

	Animator _animator;

	//LO QUE DEBERIA ESTAR EN EL MODEL
	float currentHP;

	//LO QUE NO DEBERIA ESTAR(o no estar aca al menso)
	PlayerModel _model;

	public PlayerView(float maxHP)
	{
		this.maxHP = maxHP;
		currentHP = maxHP;
	}

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_model = new PlayerModel(transform, movementSpeed, aimPointer, aimSensitivity, this, maxHP);
		myHabilitiesManager = new HabilitiesManager(myHabilities, this);
		currentHP = maxHP;
		EventsManager.SuscribeToEvent("FireHabilitie", ThrowSpellAnimation);
	}

	public void UpdateMovementAnimation()
	{
		if (_model.GetCurrentDir() != Vector3.zero)
			_animator.SetFloat("Speed", 1);
		else
		{
			_animator.SetFloat("Speed", 0);
			return;
		}
		_animator.SetFloat("zAxis", _model.GetCurrentDir().z);
		_animator.SetFloat("xAxis", _model.GetCurrentDir().x);
	}

	public void UpdateHealthBar()
	{
		healthBar.fillAmount = _model.GetCurrentHP() / _model.GetMaxHp();
	}

	public void ThrowSpellAnimation(params object[] parameters)
	{
		_animator.SetTrigger("Attack");
	}

	//
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	public void SetHealth(float health)
	{
		_model.SetHealth(health);
		/*
		if (health >= maxHP)
			currentHP = maxHP;
		else if (health <= 0)
			currentHP = 0;
		else
			currentHP = health;

		DamageOrHealAnimation(health);
		healthBar.fillAmount = currentHP / maxHP;
		*/
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
		return _model.GetCurrentHP();
	}

	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	//ESTO ES LOGICA SACARLO DE LA VIEW!
	public float GetMaxHP()
	{
		return _model.GetMaxHp();
	}
}
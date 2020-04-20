using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView
{
	//Tal vez en un futuro haya que dividir en distintas clases:
	// -La que se encargue del animator,
	// -La que se encargue de las particulas,
	// -etc..
	Image _healthBar;
	Animator _animator;

	PlayerModel _model;

	public PlayerView(Animator animator, Image healthBar)
	{
		_animator = animator;
		_healthBar = healthBar;
		EventsManager.SuscribeToEvent("FireHabilitie", SetSpellCastAnimation);
	}

	public void UpdateMovementAnimation(Vector3 axis)
	{
		if (!IsChangingPosition(axis))
		{
			SetIdleAnimation();
			return;
		}
		SetMovingAnimation(axis);
	}

	public void UpdateHealthBar(float fillAmount)
	{
		_healthBar.fillAmount = fillAmount;
	}

	public void SetSpellCastAnimation(params object[] parameters)
	{
		_animator.SetTrigger("Attack");
	}

	private void SetMovingAnimation(Vector3 axis)
	{
		_animator.SetFloat("Speed", 1);
		_animator.SetFloat("zAxis", axis.z);
		_animator.SetFloat("xAxis", axis.x);
	}

	private void SetIdleAnimation()
	{
		_animator.SetFloat("Speed", 0);
	}

	private bool IsChangingPosition(Vector3 axis)
	{
		if (axis != Vector3.zero)
			return true;
		return false;
	}

	private void HealthAnimation()
	{
		Debug.LogWarning("Heal animation its not already implemented");
	}

	private void DamagedAnimation()
	{
		Debug.LogWarning("Damaged animation its not already implemented");
	}
}
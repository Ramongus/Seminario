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
	Animator _canvasAnimator;

	PlayerModel _model;

	public PlayerView(Animator animator, Image healthBar, Animator canvasAnimator)
	{
		_animator = animator;
		_healthBar = healthBar;
		_canvasAnimator = canvasAnimator;
		//EventsManager.SuscribeToEvent("FireHabilitie", SetSpellCastAnimation);
		EventsManager.SuscribeToEvent("AbilitieCasted", SetSpellCastAnimation);
		EventsManager.SuscribeToEvent("CastTripleAttack", SetTripleRockAnimation);
		EventsManager.SuscribeToEvent("PlayerResurrect", SetIdleAnimation);
	}

	public void UpdateMovementAnimation(Vector3 axis)
	{
		if (!IsChangingPosition(axis))
		{
			SetMovingAnimation(Vector3.zero);
			return;
		}
		SetMovingAnimation(axis);
	}

	public void UpdateHealthBar(float fillAmount)
	{
		if (_healthBar == null) return;
		_healthBar.fillAmount = fillAmount;
	}

	public void DamagedVignetteAnimation()
	{
		_canvasAnimator.SetTrigger("Damaged");
	}

	public void SetSpellCastAnimation(params object[] parameters)
	{
		_animator.SetTrigger("Attack");
	}

	public void SetTripleRockAnimation(params object[] parameters)
	{
		_animator.SetTrigger("TripleAttack");
	}

	public void DieAnimation()
	{
		_animator.SetTrigger("Die");
	}

	public void FallingAnimation()
	{
		//_animator.SetTrigger("Falling");
	}

	public void SetIdleAnimation(params object[] parameters)
	{
		_animator.SetTrigger("Idle");
	}

	private void SetMovingAnimation(Vector3 axis)
	{
		if(IsChangingPosition(axis))
			_animator.SetFloat("Speed", 1);
		else
			_animator.SetFloat("Speed", 0);
		_animator.SetFloat("zAxis", axis.z);
		_animator.SetFloat("xAxis", axis.x);
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
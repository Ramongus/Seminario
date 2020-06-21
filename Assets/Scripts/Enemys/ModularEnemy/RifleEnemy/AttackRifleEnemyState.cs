using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRifleEnemyState : MonoBehaviour, IStateMono
{
	[SerializeField] string stateName;
	[SerializeField] float timeToAttack;

	float timer;

	StateMachine myStateMachine;

	RifleEnemy rifleEnemyData;

	[SerializeField] float changeColorTime;
	[SerializeField] Color changeColor;
	Color defaultColor;
	float changeColorTimer;

	private void Start()
	{
		SetStateMachine();
		rifleEnemyData = GetComponent<RifleEnemy>();
		timer = timeToAttack;
	}

	public string GetStateName()
	{
		return stateName;
	}

	public void SetStateMachine()
	{
		myStateMachine = GetComponent<StateMachine>() ?? throw new MissingComponentException("THERE IS NO STATE MACHINE ATTACHED TO THE OBJECT");
	}

	public void StateAwake()
	{
		changeColorTimer = changeColorTime;
		timer = timeToAttack;
		defaultColor = rifleEnemyData.GetLineColor();
		rifleEnemyData.SetIsAttacking(true);
	}

	public void StateExecute()
	{
		changeColorTimer -= Time.deltaTime;
		if(changeColorTimer <= 0)
		{
			if(defaultColor == rifleEnemyData.GetLineColor())
				rifleEnemyData.SetLineColor(changeColor);
			else
				rifleEnemyData.SetLineColor(defaultColor);
			changeColorTimer = changeColorTime;
		}

		timer -= Time.deltaTime;
		if(timer <= 0)
		{
			RaycastHit hit;
			if(Physics.Raycast(transform.position, transform.forward, out hit, rifleEnemyData.GetAttackRange()))
			{
				Player player = hit.collider.GetComponent<Player>();
				if (player != null)
				{
					player.SetHealth(player.GetHealth() - rifleEnemyData.GetDamage());
				}
			}
			timer = timeToAttack;
			rifleEnemyData.SetLineColor(new Color(0f, 0f, 0f, 0f));
			myStateMachine.SetState<DissapearState>();
		}
	}

	public void StateSleep()
	{
		rifleEnemyData.SetIsAttacking(false);
	}
}

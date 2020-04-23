using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingAttackState : MonoBehaviour, IState
{
	[SerializeField] string stateName;
	StateMachine myStateMachine;

	[SerializeField] float timeCharging;
	[SerializeField] float attackRange;
	float timer;

	[SerializeField] Transform lineCastPoint;

	Transform target;

	RifleEnemy rifleEnemyData;

	private void Start()
	{
		rifleEnemyData = GetComponent<RifleEnemy>();
		SetStateMachine();
		timer = timeCharging;
		target = FindObjectOfType<Player>().transform;
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
		timer = timeCharging;
	}

	public void StateExecute()
	{
		timer -= Time.deltaTime;
		float opacityLineAmount = (timeCharging - timer) / timeCharging;
		rifleEnemyData.SetOpacityToMaterial(opacityLineAmount);
		Debug.Log(opacityLineAmount);
		if (timer <= 0)
		{
			timer = timeCharging;
			myStateMachine.SetState<AttackRifleEnemyState>();
		}
	}

	public void StateSleep()
	{
		
	}
}

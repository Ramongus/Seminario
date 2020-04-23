using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : MonoBehaviour, IState
{
	[SerializeField] string stateName;
	[SerializeField] float damage;
	[SerializeField] Collider meleeAttackArea;

	[SerializeField] float timeToChangeState;
	float timer;

	StateMachine myStateMachine;

	private void Start()
	{
		SetStateMachine();
	}

	public string GetStateName()
	{
		return stateName;
	}

	public void SetStateMachine()
	{
		myStateMachine = GetComponent<StateMachine>();
	}

	public void StateAwake()
	{
		Debug.Log("Attack State");
		timer = timeToChangeState;
		GetComponent<Animator>().SetTrigger("Attack");
	}

	public void StateExecute()
	{
		timer -= Time.deltaTime;
		if(timer <= 0)
		{
			myStateMachine.SetState<ChaseEnemyState>();
		}
	}

	public void StateSleep()
	{
		meleeAttackArea.enabled = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		Player player = other.GetComponent<Player>();
		if(player != null)
		{
			player.SetHealth(player.GetHealth() - damage);
		}
		meleeAttackArea.enabled = false;
	}

	public void Attack()
	{
		meleeAttackArea.enabled = true;
	}
}

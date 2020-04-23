using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : MonoBehaviour, IState
{
	[SerializeField] string stateName;

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
	}

	public void StateExecute()
	{
		
	}

	public void StateSleep()
	{
		
	}
}

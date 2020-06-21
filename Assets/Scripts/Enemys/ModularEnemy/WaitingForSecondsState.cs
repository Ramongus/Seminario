using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForSecondsState : MonoBehaviour, IStateMono
{
	[SerializeField] string stateName;
	[SerializeField] float timeToWait;
	[SerializeField] string nextStateName;
	float timer;

	StateMachine myStateMachine;

	private void Awake()
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
		timer = timeToWait;
	}

	public void StateExecute()
	{
		timer -= Time.deltaTime;
		if (timer <= 0)
			myStateMachine.SetStateByName(nextStateName);
	}

	public void StateSleep()
	{
		timer = timeToWait;
	}

}

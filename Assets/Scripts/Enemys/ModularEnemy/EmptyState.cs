using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyState : MonoBehaviour, IStateMono
{
	[SerializeField] string stateName;

	public string GetStateName()
	{
		return stateName;
	}

	public void SetStateMachine()
	{
		throw new System.NotImplementedException();
	}

	public void StateAwake()
	{
		
	}

	public void StateExecute()
	{
		
	}

	public void StateSleep()
	{
		
	}
}

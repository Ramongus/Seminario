using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBombsState : MonoBehaviour, IStateMono
{
	[SerializeField] string stateName;
	[SerializeField] TripleBombAttack bombCaster;
	[SerializeField] Transform bombSpawnPoint;
	[SerializeField] float timeToDissapear;
	float timer;
	Transform target;
	StateMachine myStateMachine;

	protected void Awake()
	{
		SetStateMachine();
		target = FindObjectOfType<Player>().transform;
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
		timer = timeToDissapear;
		TripleBombAttack bombCaster = Instantiate(this.bombCaster);
		bombCaster.SetTarget(target);
		bombCaster.transform.position = bombSpawnPoint.position;
		bombCaster.transform.parent = bombSpawnPoint;
	}

	public void StateExecute()
	{
		timer -= Time.deltaTime;
		if(timer <= 0)
		{
			myStateMachine.SetStateByName("Dissapear");
		}
	}

	public void StateSleep()
	{
		
	}
}

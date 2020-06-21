using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSpawnPointFromTarget : MonoBehaviour, IStateMono
{
	[SerializeField] string stateName;
	[SerializeField] float minDistanceToTarget;
	[SerializeField] float maxDistanceToTarget;

	[SerializeField] Transform target;

	[SerializeField] float timeChoosingPos;
	float timer;

	List<ANode> allnodes;
	List<ANode> candidatesNodes;

	StateMachine myStateMachine;

	private void Awake()
	{
		SetStateMachine();
		candidatesNodes = new List<ANode>();
		allnodes = FindObjectOfType<NodesList>().GetNodes();
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
		timer = timeChoosingPos;
		foreach (ANode node in allnodes)
		{
			float distance = Vector3.Distance(node.transform.position, target.position);
			if(distance >= minDistanceToTarget && distance <= maxDistanceToTarget)
			{
				candidatesNodes.Add(node);
			}
		}
		if(candidatesNodes.Count > 0)
		{
			Vector3 randPoint = candidatesNodes[Random.Range(0, candidatesNodes.Count)].transform.position;
			transform.position = randPoint;
		}

	}

	public void StateExecute()
	{
		timer -= Time.deltaTime;
		if(timer <= 0)
		{
			myStateMachine.SetStateByName("Spawn");
		}
	}

	public void StateSleep()
	{
		candidatesNodes = new List<ANode>();
		timer = timeChoosingPos;
	}
}

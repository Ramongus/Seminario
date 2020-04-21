using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNearObjective : State
{
	WaypointsUtilities waypointsUtilities;
	List<ANode> nodes;
	AStar pathfinder;
	Transform owner;
	float distanceToMoveArround;

	Transform objective;

	List<Transform> path;

	int index;

	float speed;

	public MoveNearObjective(StateMachine sm, List<ANode> availableNodes, Transform owner, Transform objective, float distanceToMoveArround, float speed) : base(sm)
	{
		nodes = new List<ANode>(availableNodes);
		pathfinder = new AStar();
		this.owner = owner;
		this.distanceToMoveArround = distanceToMoveArround;
		this.objective = objective;
		index = 0;
		waypointsUtilities = new WaypointsUtilities();
		this.speed = speed;
	}

	public override void Awake()
	{
		Debug.LogWarning("SE ENTRO EN EL ESTADO MOVE NEAR OBJECTIVE");

		ANode finalNode = GetFinalNode();
		ANode initialNode = GetInitialNode();
		if(finalNode != null && initialNode != null)
		{
			pathfinder.SeTFinalNode(finalNode);
			pathfinder.SetInitialNode(initialNode);
		}
		Debug.Log(finalNode.name);
		Debug.Log(initialNode.name);

		Stack<ANode> pathInStack = new Stack<ANode>(pathfinder.GetPath());
		Debug.Log("Stack size: " + pathInStack.Count);
		path = ConvertNodeStackToTransformList(pathInStack);
		index = 1;
	}

	public override void Execute()
	{

	}

	private List<Transform> ConvertNodeStackToTransformList(Stack<ANode> pathInStack)
	{
		List<Transform> list = new List<Transform>();
		while(pathInStack.Count > 0)
		{
			list.Add(pathInStack.Pop().transform);
		}
		return list;
	}

	private ANode GetFinalNode()
	{
		ANode candidateNode = null;
		float distanceDifference = Mathf.Infinity;
		foreach (ANode node in nodes)
		{
			float difference = GetDistanceDifference(node);
			if(difference < distanceDifference)
			{
				distanceDifference = difference;
				candidateNode = node;
			}
		}
		return candidateNode;
	}

	private float GetDistanceDifference(ANode node)
	{
		float distanceToNode = (node.transform.position - objective.position).magnitude;
		return Mathf.Abs(distanceToNode - distanceToMoveArround);
	}

	private ANode GetInitialNode()
	{
		ANode candidateNode = null;
		float nearestDistance = Mathf.Infinity;

		foreach (ANode node in nodes)
		{
			float distance = GetOwnerDistanceToNode(node);
			if(distance < nearestDistance)
			{
				nearestDistance = distance;
				candidateNode = node;
			}
		}
		return candidateNode;
	}

	private float GetOwnerDistanceToNode(ANode node)
	{
		return Vector3.Distance(owner.position, node.transform.position);
	}
}

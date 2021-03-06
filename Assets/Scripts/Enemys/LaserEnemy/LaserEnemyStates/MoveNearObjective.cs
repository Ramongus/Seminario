﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveNearObjective : State
{
	WaypointsUtilities waypointsUtilities;
	List<ANode> nodes;
	MyAStar pathfinder;
	Transform owner;
	float distanceToMoveArround;

	Transform objective;

	List<Transform> path;

	int index;

	float speed;

	public MoveNearObjective(StateMachineClassic sm, List<ANode> availableNodes, Transform owner, Transform objective, float distanceToMoveArround, float speed) : base(sm)
	{
		nodes = new List<ANode>(availableNodes);
		//pathfinder = new AStar();
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
		Debug.Log("Final node: " + finalNode.name);
		Debug.Log("Initial node" + initialNode.name);

		if(finalNode != null && initialNode != null)
		{
			//pathfinder.SeTFinalNode(finalNode);
			//pathfinder.SetInitialNode(initialNode);
		}

		//Stack<ANode> pathInStack = new Stack<ANode>(pathfinder.GetPath());
		List<ANode> pathNodes = new List<ANode>(pathfinder.GetPath().ToArray());
		//Debug.Log("Stack size: " + pathInStack.Count);
		path = ConvertNodeListToTransformList(pathNodes);
	}

	public override void Execute()
	{
		float range = 4f;
		Vector3 nodePositionAtSamehight = GetNodeYAtSameHight();
		if(Vector3.Distance(nodePositionAtSamehight, owner.transform.position) < range)
		{
			index++;
			if(index >= path.Count)
			{
				_sm.SetState<ChargeAttack>();
				return;
			}
		}

		//Vector3 deltaDir = (path[index].position - owner.transform.position);
		//Vector3 currentDir = new Vector3(deltaDir.x, 0, deltaDir.z).normalized;
		Vector3 currentDir = Vector3.Normalize(nodePositionAtSamehight - owner.transform.position);
		owner.forward = currentDir;
		owner.transform.position += speed * owner.transform.forward * Time.deltaTime;
	}

	private Vector3 GetNodeYAtSameHight()
	{
		return new Vector3(path[index].position.x, owner.transform.position.y, path[index].position.z);
	}

	private List<Transform> ConvertNodeListToTransformList(List<ANode> nodeList)
	{
		List<Transform> list = new List<Transform>();
		foreach (ANode node in nodeList)
		{
			list.Add(node.transform);
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
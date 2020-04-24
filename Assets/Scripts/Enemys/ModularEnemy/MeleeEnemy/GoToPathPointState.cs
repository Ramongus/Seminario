using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPathPointState : MonoBehaviour, IState
{
	[SerializeField] string stateName;
	[SerializeField] float rotationSpeed;
	[SerializeField] Transform raycastInitialPoint;
	[SerializeField] LayerMask raycastLayerMask;

	List<ANode> nodes;
	List<Transform> path;

	StateMachine myStateMachine;

	int index;

	IMoveBehaviour moveBehaviour;

	Transform target;

	AStar pathfinder;

	Animator animator;

	private void Start()
	{
		SetStateMachine();
		target = FindObjectOfType<Player>().transform;
		pathfinder = new AStar();
		nodes = new List<ANode>(FindObjectOfType<NodesList>().GetNodes());
		moveBehaviour = GetComponent<IMoveBehaviour>();
		animator = GetComponent<Animator>();
	}

	public string GetStateName()
	{
		return stateName;
	}

	public void SetStateMachine()
	{
		myStateMachine = GetComponent<StateMachine>() ?? throw new MissingComponentException("Non state machine component attached");
	}

	public void StateAwake()
	{
		//moveBehaviour.SetVelocity(transform.forward);
		index = 0;
		path = GetNewPath();
		foreach (var item in path)
		{
			Debug.Log(item);
		}
	}

	public void StateExecute()
	{
		Debug.Log("ON PATH STATE");
		Vector3 toTargetFromRayPoint = target.position - raycastInitialPoint.position;
		RaycastHit hit;
		if(Physics.Raycast(raycastInitialPoint.position, new Vector3(toTargetFromRayPoint.x, 0, toTargetFromRayPoint.z).normalized, out hit, Mathf.Infinity, raycastLayerMask))
		{
			Player player = hit.collider.GetComponent<Player>();
			if(player != null)
			{
				myStateMachine.SetState<ChaseEnemyState>();
				return;
			}
		}
		if (IsEndPath())
		{
			path = GetNewPath();
			index = 0;
		}

		FollowPath();
	}

	private void FollowPath()
	{
		float range = 2f;
		Vector3 toPathIndex = path[index].position - transform.position;
		float distanceToNextWaypoint = toPathIndex.magnitude;
		if(distanceToNextWaypoint < range)
		{
			index++;
			return;
		}
		Vector3 dirInterpolation = Vector3.Lerp(transform.forward, new Vector3(toPathIndex.x, 0, toPathIndex.z).normalized, rotationSpeed * Time.deltaTime);
		transform.forward = new Vector3(dirInterpolation.x, 0, dirInterpolation.z).normalized;
		moveBehaviour.SetVelocity(transform.forward);
		animator.SetFloat("Speed", 1);
	}

	private List<Transform> GetNewPath()
	{
		pathfinder.SeTFinalNode(GetFinalNode());
		pathfinder.SetInitialNode(GetInitialNode());
		List<ANode> pathNodes = new List<ANode>(pathfinder.GetPath().ToArray());
		return ConvertNodeListToTransformList(pathNodes);
	}

	private bool IsEndPath()
	{
		if(index >= path.Count)
		{
			return true;
		}
		return false;
	}

	public void StateSleep()
	{
		index = 0;
	}

	public void SetPath(List<Transform> path)
	{
		this.path = new List<Transform>(path);
	}

	private ANode GetFinalNode()
	{
		ANode candidateNode = null;
		float distanceDifference = Mathf.Infinity;
		foreach (ANode node in nodes)
		{
			float difference = GetDistanceDifference(node);
			if (difference < distanceDifference)
			{
				distanceDifference = difference;
				candidateNode = node;
			}
		}
		return candidateNode;
	}

	private float GetDistanceDifference(ANode node)
	{
		float distanceToNode = (node.transform.position - target.position).magnitude;
		return Mathf.Abs(distanceToNode);
	}

	private ANode GetInitialNode()
	{
		ANode candidateNode = null;
		float nearestDistance = Mathf.Infinity;

		foreach (ANode node in nodes)
		{
			float distance = GetOwnerDistanceToNode(node);
			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				candidateNode = node;
			}
		}
		return candidateNode;
	}

	private float GetOwnerDistanceToNode(ANode node)
	{
		return Vector3.Distance(transform.position, node.transform.position);
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPathPointState : MonoBehaviour, IState, IUsePathfinding
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
	Animator animator;

	private void Start()
	{
		SetStateMachine();
		target = FindObjectOfType<Player>().transform;
		nodes = new List<ANode>(FindObjectOfType<NodesList>().GetNodes());
		moveBehaviour = GetComponent<IMoveBehaviour>();
		animator = GetComponent<Animator>();
	}

	public void StateAwake()
	{
		index = 0;
		SetNewPath();
	}

	public void StateExecute()
	{
		Vector3 toTargetFromRayPoint = target.position - raycastInitialPoint.position;
		Debug.DrawLine(raycastInitialPoint.position, raycastInitialPoint.position + new Vector3(toTargetFromRayPoint.x, 0, toTargetFromRayPoint.z).normalized * 100);
		RaycastHit hit;
		if(Physics.Raycast(raycastInitialPoint.position, new Vector3(toTargetFromRayPoint.x, 0, toTargetFromRayPoint.z).normalized, out hit, 100, raycastLayerMask))
		{
			Player player = hit.collider.GetComponent<Player>();
			if(player != null)
			{
				myStateMachine.SetState<ChaseEnemyState>();
				return;
			}
		}

		if (path != null)
		{
			if (IsEndPath())
			{
				SetNewPath();
				index = 0;
			}
			FollowPath();
		}
	}

	public void StateSleep()
	{
		index = 0;
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

	private void SetNewPath()
	{
		path = null;
		AStar.Instance.AddRequester(this);
	}

	private bool IsEndPath()
	{
		if(index >= path.Count)
		{
			return true;
		}
		return false;
	}

	public void SetPath(Stack<ANode> path)
	{
		this.path = ConvertNodeListToTransformList(new List<ANode>(path.ToArray()));
	}

	public ANode GetFinalNode()
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
		if(candidateNode == null)
		{
			Debug.LogError("ES EL FINAL NODE QUE ESTA DEVOLVIENDO NULO!");
		}
		return candidateNode;
	}

	private float GetDistanceDifference(ANode node)
	{
		float distanceToNode = (node.transform.position - target.position).magnitude;
		return Mathf.Abs(distanceToNode);
	}

	public ANode GetInitialNode()
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
		if (candidateNode == null)
		{
			Debug.LogError("ES EL #INITIAL# NODE QUE ESTA DEVOLVIENDO NULO!");
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

	public string GetStateName()
	{
		return stateName;
	}

	public void SetStateMachine()
	{
		myStateMachine = GetComponent<StateMachine>() ?? throw new MissingComponentException("Non state machine component attached");
	}
}

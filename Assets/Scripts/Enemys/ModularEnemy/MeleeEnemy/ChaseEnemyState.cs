using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemyState : MonoBehaviour, IStateMono
{
	[SerializeField] string stateName;
	[SerializeField] float rotationSpeed;
	[SerializeField] Transform raycastInitialPoint;
	[SerializeField] float attackRange;
	[SerializeField] float sightViewAngle;
	[SerializeField] LayerMask layerRay;
	[SerializeField] float avoidanceAmount;
	[SerializeField] float obstacleRadiusDetection;
	StateMachine myStateMachine;

	Transform target;
	IMoveBehaviour moveBehaviour;

	Animator animator;

	private void Start()
	{
		SetStateMachine();
		target = FindObjectOfType<Player>().transform;
		moveBehaviour = GetComponent<IMoveBehaviour>();
		animator = GetComponent<Animator>();
	}

	public string GetStateName()
	{
		return stateName;
	}

	public void SetStateMachine()
	{
		myStateMachine = GetComponent<StateMachine>() ?? throw new MissingComponentException();
	}

	public void StateAwake()
	{
	}

	public void StateExecute()
	{
		Vector3 toTarget = target.position - transform.position;
		Vector3 toTargetIgnoringHeightDir = new Vector3(toTarget.x, 0, toTarget.z).normalized;
		if (toTarget.magnitude < attackRange)
		{
			if (IsInSightView(toTargetIgnoringHeightDir))
			{
				myStateMachine.SetState<MyMeleeAttackState>();
				return;
			}
		}

		Vector3 toTargetFromRayPoint = target.position - raycastInitialPoint.position;
		RaycastHit hit;
		if (Physics.Raycast(raycastInitialPoint.position, new Vector3(toTargetFromRayPoint.x, 0, toTargetFromRayPoint.z).normalized, out hit, Mathf.Infinity, layerRay))
		{
			Player player = hit.collider.GetComponent<Player>();
			if (player == null)
			{
				myStateMachine.SetState<GoToPathPointState>();
				return;
			}
		}

		Vector3 avoidanceDir = GetObstacleAvoidanceDir();
		Vector3 avoidanceDirHeightIgnored = new Vector3(avoidanceDir.x, 0, avoidanceDir.z).normalized;
		Vector3 finalAvoidance = avoidanceDirHeightIgnored * avoidanceAmount;
		Debug.DrawLine(transform.position, transform.position + finalAvoidance, Color.yellow);
		transform.forward = Vector3.Lerp(transform.forward, (toTargetIgnoringHeightDir + finalAvoidance).normalized, rotationSpeed * Time.deltaTime);
		moveBehaviour.SetVelocity(transform.forward);
		animator.SetFloat("Speed", 1);
	}

	private Vector3 GetObstacleAvoidanceDir()
	{
		Collider[] nearObjects = Physics.OverlapSphere(transform.position, obstacleRadiusDetection);
		List<Collider> obstaclesFiltered = new List<Collider>();
		for (int i = 0; i < nearObjects.Length; i++)
		{
			if (nearObjects[i].gameObject.layer == 12)
			{
				obstaclesFiltered.Add(nearObjects[i]);
				Debug.Log(nearObjects[i]);
			}
		}
		Vector3 finalDir = new Vector3();
		foreach (var item in obstaclesFiltered)
		{
			finalDir += (transform.position - item.ClosestPoint(transform.position));
		}
		return finalDir.normalized;
	}

	private bool IsInSightView(Vector3 targetDir)
	{
		if (Vector3.Angle(transform.forward, targetDir) < sightViewAngle)
			return true;
		return false;
	}

	public void StateSleep()
	{
		moveBehaviour.SetVelocity(Vector3.zero);
		animator.SetFloat("Speed", 0);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + transform.forward * obstacleRadiusDetection);
	}
}

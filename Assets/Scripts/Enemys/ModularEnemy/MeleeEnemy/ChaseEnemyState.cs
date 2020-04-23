using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemyState : MonoBehaviour, IState
{
	[SerializeField] string stateName;
	[SerializeField] float rotationSpeed;
	[SerializeField] Transform raycastInitialPoint;
	[SerializeField] float attackRange;
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
		Debug.Log("ON CHASE STATE");
		Vector3 toTarget = target.position - transform.position;
		if(toTarget.magnitude < attackRange)
		{
			myStateMachine.SetState<MeleeAttackState>();
			return;
		}

		Vector3 toTargetFromRayPoint = target.position - raycastInitialPoint.position;
		RaycastHit hit;
		if (Physics.Raycast(raycastInitialPoint.position, new Vector3(toTargetFromRayPoint.x, 0, toTargetFromRayPoint.z).normalized, out hit))
		{
			Player player = hit.collider.GetComponent<Player>();
			if (player == null)
			{
				myStateMachine.SetState<GoToPathPointState>();
				return;
			}
		}
		transform.forward = Vector3.Lerp(transform.forward, toTarget.normalized, rotationSpeed * Time.deltaTime);
		moveBehaviour.SetVelocity(transform.forward);
		animator.SetFloat("Speed", 1);
	}

	public void StateSleep()
	{
		Debug.Log("StateSleeps - Chase");
		moveBehaviour.SetVelocity(Vector3.zero);
		animator.SetFloat("Speed", 0);
	}
}

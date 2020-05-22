using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSurrounding : AbstractAbilities
{
	[Header("Ball Ssurround Values")]
	[SerializeField] float rangeDetection;
	[SerializeField] float distanceFromOwner;
	[SerializeField] public float circleRotationSpeed;
	[SerializeField] float upDownMovementRadius;
	[SerializeField] float upDownSpeed;
	[SerializeField] public Transform owner;
	[SerializeField] LayerMask enemyAndPlayerLayerMask;
	[SerializeField] int enemysLayerNumber;
	[SerializeField] float attackSpeed;
	[SerializeField] Vector3 scaleOnAtack;
	[SerializeField] public float rotateSelfSpeed;
	Vector3 rotateSelfDirection;
	Transform target;
	public float t;
	public float t2;

	enum SurroundingBallState { Idle, Attack, Attacking}
	SurroundingBallState currentState;

	public void SetInitialCircleAngle(float angle)
	{
		t = Mathf.Deg2Rad * angle;
	}

	public void SetInitialUpDownAngle(float angle)
	{
		t2 = Mathf.Deg2Rad * angle;
	}

	protected override void Awake()
	{
		base.Awake();
		currentState = SurroundingBallState.Idle;
		rotateSelfDirection = new Vector3(UnityEngine.Random.Range(0, 9), UnityEngine.Random.Range(0, 9), UnityEngine.Random.Range(0, 9)).normalized;
	}

	protected override void Update()
	{
		base.Update();
		switch (currentState)
		{
			case SurroundingBallState.Idle:
				SurroundingMovement();
				if (IsEnemyOnRange())
					if (IsClearAttack())
						currentState = SurroundingBallState.Attack;
				break;
			case SurroundingBallState.Attack:
				Attack();
				break;
			default:
				break;
		}
	}

	private void SurroundingMovement()
	{
		t += Time.deltaTime;
		t2 += Time.deltaTime;
		float circleSin = Mathf.Sin(t * circleRotationSpeed);
		float circleCos = Mathf.Cos(t * circleRotationSpeed);
		float upDownSin = Mathf.Sin(t2 * upDownSpeed);
		transform.Rotate(rotateSelfDirection * rotateSelfSpeed);
		transform.position = owner.position + new Vector3(circleSin, 0, circleCos) * distanceFromOwner + Vector3.up * initialHeight + Vector3.up * upDownSin * upDownMovementRadius;
	}

	private bool IsEnemyOnRange()
	{
		var colliders = Physics.OverlapSphere(this.transform.position, rangeDetection);
		foreach (var item in colliders)
		{
			if(item.gameObject.layer == enemysLayerNumber)//EnemyLayer
			{
				target = item.transform;
				return true;
			}
		}
		return false;
	}

	private bool IsClearAttack()
	{
		Vector3 dirToTargetIgnoringHeight = (target.position - transform.position).normalized;
		dirToTargetIgnoringHeight = new Vector3(dirToTargetIgnoringHeight.x, 0, dirToTargetIgnoringHeight.z).normalized;
		RaycastHit hit;
		if(Physics.Raycast(transform.position, dirToTargetIgnoringHeight, out hit, rangeDetection, enemyAndPlayerLayerMask))
		{
			if (hit.collider.gameObject.layer == enemysLayerNumber) return true;
		}
		return false;
	}

	private void Attack()
	{
		Rigidbody rigi = GetComponent<Rigidbody>();
		Vector3 targetDir = (target.position - transform.position).normalized;
		targetDir = new Vector3(targetDir.x, 0, targetDir.z).normalized;
		transform.localScale = scaleOnAtack;
		transform.forward = targetDir;
		rigi.AddForce(targetDir * attackSpeed, ForceMode.Impulse);
		currentState = SurroundingBallState.Attacking;
		//transform.position += targetDir * attackSpeed * Time.deltaTime;
	}

	public override void SetInitiation(Vector3 castPos, Vector3 playerPos)
	{
		
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, rangeDetection);
	}

}

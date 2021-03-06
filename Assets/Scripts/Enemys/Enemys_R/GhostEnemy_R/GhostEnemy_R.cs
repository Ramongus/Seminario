﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy_R : BaseEnemy_R_Damagable
{
	public Player target;
	public float speed;
	public float rotationSpeed;
	public Rigidbody rigi;
	public LayerMask obstaclesLayer;
	public Transform raySpawnPoint;
	public float radiusForNearObstacles;
	public float attackRange;
	public float avoidanceWeight;

	Vector3 initialPosition;
	Quaternion initialRotation;
	StateMachineClassic sm;

	protected override void Awake()
	{
		base.Awake();
		initialPosition = transform.position;
		initialRotation = transform.rotation;
		rigi = GetComponent<Rigidbody>();
		target = FindObjectOfType<Player>();
		EventsManager.SuscribeToEvent("PlayerResurrect", SetInitialPosAndRotation);

		sm = new StateMachineClassic();
		sm.AddState(new GhostEnemy_R_ChaseState(sm, this));
	}

	private void SetInitialPosAndRotation(object[] parameters)
	{
		transform.position = initialPosition;
		transform.rotation = initialRotation;
	}

	protected override void Update()
	{
		base.Update();
		if(GetTargetDistance() <= attackRange)
			KillTarget();
		sm.Update();
	}

	private void KillTarget()
	{
		target.SetHealth(0);
		Destroy(this.gameObject);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.3f);
		Gizmos.DrawSphere(transform.position, radiusForNearObstacles);
	}

	private float GetTargetDistance()
	{
		Vector3 targetIgnoringHeight = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
		return Vector3.Distance(transform.position, targetIgnoringHeight);
	}
}

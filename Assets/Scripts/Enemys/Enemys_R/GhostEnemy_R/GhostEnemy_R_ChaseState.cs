using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy_R_ChaseState : State
{
	GhostEnemy_R owner;

	private Transform obstacle;

	public GhostEnemy_R_ChaseState(StateMachineClassic sm, GhostEnemy_R owner) : base(sm)
	{
		this.owner = owner;
	}

	public override void Execute()
	{
		base.Execute();
		Chase();
	}

	private void Chase()
	{
		Vector3 toTarget = owner.target.transform.position - owner.transform.position;
		Vector3 dir = toTarget.normalized;
		dir = new Vector3(dir.x, 0, dir.z).normalized;
		GetObstacle(dir);
		if(obstacle != null)
		{
			Vector3 opDirOfObstacle = (owner.transform.position - obstacle.transform.position).normalized;
			dir += (new Vector3(opDirOfObstacle.x, 0, opDirOfObstacle.z).normalized * owner.avoidanceWeight).normalized;
		}
		owner.transform.forward = Vector3.Lerp(owner.transform.forward, dir, owner.rotationSpeed * Time.deltaTime);
		owner.rigi.velocity = owner.transform.forward * owner.speed;
	}

	private void GetObstacle(Vector3 dir)
	{
		Collider[] obstaclesNear;
		obstaclesNear = Physics.OverlapSphere(owner.transform.position, owner.radiusForNearObstacles, owner.obstaclesLayer);
		if (obstaclesNear.Length > 0)
		{
			Debug.Log("Hay un obstaculo cerca");
			float distanceToNearest = Mathf.Infinity;
			foreach (var item in obstaclesNear)
			{
				float distance = Vector3.Distance(item.transform.position, owner.transform.position);
				if (distance < distanceToNearest)
				{
					distanceToNearest = distance;
					obstacle = item.transform;
				}
			}
			return;
		}
		obstacle = null;
		return;
	}
}

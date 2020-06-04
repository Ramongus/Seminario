using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy_R : BaseEnemy_R_Damagable
{
	StateMachineClassic sm;
	Animator anim;

	Player target;

	[Header("Casting values")]
	public float castingTime;

	[Header("Attack values")]
	public LayerMask obstaclesLayer;
	public Transform raySpawnPoint;
	public float range;

	protected override void Awake()
	{
		base.Awake();
		sm = new StateMachineClassic();
		sm.AddState(new LaserEnemy_R_CastLaserState(sm, this));
		sm.AddState(new LaserEnemy_R_IdleState(sm, this));
		sm.SetState<LaserEnemy_R_IdleState>();
		anim = GetComponent<Animator>();
		target = FindObjectOfType<Player>();
	}

    void Update()
    {
		transform.forward = (target.transform.position - transform.position).normalized;
		if (Vector3.Distance(target.transform.position, transform.position) <= range) SetCastingAttackState();
		else SetIdleState();
		sm.Update();
    }

	public void Attack()
	{
		SetAttackAnimation();
		RaycastHit hit;
		if (!Physics.Raycast(raySpawnPoint.position, transform.forward, out hit, range, obstaclesLayer)) target.SetHealth(0);
		else Debug.Log("El ataque fue bloqueado por: " + hit.transform.gameObject.name);
		SetIdleState();
	}

	private void SetAttackAnimation()
	{
		anim.SetTrigger("Attack");
	}

	public void SetIdleAnimation()
	{
		if (!anim) return;
		anim.SetTrigger("Idle");
	}

	public void SetCastingAnimation()
	{
		if (!anim) return;
		anim.SetTrigger("Casting");
	}

	/*
	private void OnTriggerEnter(Collider other)
	{
		Player player = other.GetComponent<Player>();
		if(player != null)
		{
			SetCastingAttackState();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Player player = other.GetComponent<Player>();
		if (player != null)
		{
			SetIdleState();
		}
	}
	*/

	private void SetIdleState()
	{
		if (!sm.IsActualState<LaserEnemy_R_IdleState>())
			sm.SetState<LaserEnemy_R_IdleState>();
	}

	private void SetCastingAttackState()
	{
		if(!sm.IsActualState<LaserEnemy_R_CastLaserState>())
			sm.SetState<LaserEnemy_R_CastLaserState>();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.3f);
		Gizmos.DrawSphere(transform.position, range);
	}
}

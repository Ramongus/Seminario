﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleArcher : AbstractEnemy
{
	public GameObject render;
	public float teleportAppearValue;
	public float teleportDisappearValue;
	public float teleportSpeed;
	public Material teleportMaterial;
	public Material golemMaterial;

	public bool testAppear;
	public bool testDisappear;

	public float minAppearDistance;
	public float maxAppearDistance;
	public Transform player;

	public float attackCooldown;

	public List<IAttack> myAttacks;

	public TripleBombAttack bombCaster;
	public Transform bombSpawnPoint;

	public float timeDisappeared;
	public float timeAppeared;
	private float timer;

	private Collider myCollider;

	Action nextAction;
	List<Action> actionsCycle;
	int actionIndex;

	Dictionary<Action, float> actionTime;

	public Animator animator;

	protected override void Awake()
	{
		base.Awake();
		player = FindObjectOfType<Player>().transform;
		SetMaterial(golemMaterial);
		myAttacks = new List<IAttack>();
		myCollider = GetComponent<Collider>();
		timer = 1;
		actionIndex = 0;

		actionsCycle = new List<Action>();
		//actionsCycle.Add(Disappear);
		//actionsCycle.Add(Appear);

		actionTime = new Dictionary<Action, float>();
		actionTime.Add(Disappear, timeDisappeared);
		actionTime.Add(Appear, timeAppeared);

		actionsCycle = new List<Action>(actionTime.Keys);
	}

	private void Update()
	{
		timer -= Time.deltaTime;
		if(timer < 0)
		{
			DoAction();
		}
		/*
		if (testAppear)
		{
			Appear();
			testAppear = false;
		}

		if (testDisappear)
		{
			Disappear();
			testDisappear = false;
		}
		*/
	}

	private void DoAction()
	{
		actionsCycle[actionIndex]();
		timer = actionTime[actionsCycle[actionIndex]];
		actionIndex++;
		if(actionIndex >= actionsCycle.Count)
		{
			actionIndex = 0;
		}
	}

	public override void Appear()
	{
		SetMaterial(teleportMaterial);
		StartCoroutine(AppearAnimation());
	}

	IEnumerator AppearAnimation()
	{
		transform.position = GetRandomValidPos();
		LookAtPlayer();
		if(teleportAppearValue > teleportDisappearValue)
		{
			while(teleportMaterial.GetFloat("_Teleport") < teleportAppearValue)
			{
				teleportMaterial.SetFloat("_Teleport", teleportMaterial.GetFloat("_Teleport") + Time.deltaTime * teleportSpeed);
				yield return null;
			}
		}
		else
		{
			while (teleportMaterial.GetFloat("_Teleport") > teleportAppearValue)
			{
				teleportMaterial.SetFloat("_Teleport", teleportMaterial.GetFloat("_Teleport") - Time.deltaTime * teleportSpeed);
				yield return null;
			}
		}
		myCollider.enabled = true;
		SetMaterial(golemMaterial);
		Attack();
	}

	private Vector3 GetRandomValidPos()
	{
		int randAngle = UnityEngine.Random.Range(0, 360);
		float randDist = UnityEngine.Random.Range(minAppearDistance, maxAppearDistance);
		Vector3 playerForward = player.forward;
		Vector3 addRandRotation = Quaternion.Euler(0, randAngle, 0) * playerForward;
		Vector3 addRandDistance = addRandRotation * randDist;
		Vector3 addInitialPlayerPos = addRandDistance + player.position;
		return addInitialPlayerPos;
	}

	public override void Disappear()
	{
		SetMaterial(teleportMaterial);
		StartCoroutine(DisappearAnimation());
	}

	private IEnumerator DisappearAnimation()
	{
		myCollider.enabled = false;
		if (teleportAppearValue < teleportDisappearValue)
		{
			while (teleportMaterial.GetFloat("_Teleport") < teleportDisappearValue)
			{
				teleportMaterial.SetFloat("_Teleport", teleportMaterial.GetFloat("_Teleport") + Time.deltaTime * teleportSpeed);
				yield return null;
			}
		}
		else
		{
			while (teleportMaterial.GetFloat("_Teleport") > teleportDisappearValue)
			{
				teleportMaterial.SetFloat("_Teleport", teleportMaterial.GetFloat("_Teleport") - Time.deltaTime * teleportSpeed);
				yield return null;
			}
		}
	}

	public override void Attack()
	{
		animator.SetTrigger("Attack");
		StartCoroutine(WaitCooldownToAttack());
	}

	IEnumerator WaitCooldownToAttack()
	{
		yield return new WaitForSeconds(attackCooldown);
		TripleBombAttack bombCaster = Instantiate(this.bombCaster);
		bombCaster.SetTarget(player);
		bombCaster.transform.position = bombSpawnPoint.position;
		bombCaster.transform.parent = bombSpawnPoint;
	}

	public override IAttack ChooseOneAttack()
	{
		int attackIndex = UnityEngine.Random.Range(0, myAttacks.Count);
		return myAttacks[attackIndex];
	}

	public override void Die()
	{
		Destroy(gameObject);
	}

	public override void Move()
	{
		throw new System.NotImplementedException();
	}

	public void SetMaterial(Material currentMaterial)
	{
		render.GetComponent<Renderer>().material = currentMaterial;
	}

	public void LookAtPlayer()
	{
		transform.forward = (player.transform.position - transform.position).normalized;
	}
}

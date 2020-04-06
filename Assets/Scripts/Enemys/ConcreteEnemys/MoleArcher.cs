using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleArcher : AbstractEnemy
{
	public GameObject render;
	public float teleportAppearValue;
	public float teleportDisappearValue;
	public float teleportSpeed;
	private Material teleportMaterial;

	public bool testAppear;
	public bool testDisappear;

	public float minAppearDistance;
	public float maxAppearDistance;
	public Transform player;

	public float attackCooldown;

	public List<IAttack> myAttacks;

	public TripleBombAttack bombCaster;

	protected override void Awake()
	{
		base.Awake();
		teleportMaterial = render.GetComponent<Renderer>().material;
		myAttacks = new List<IAttack>();
	}

	private void Update()
	{
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
	}

	public override void Appear()
	{
		StartCoroutine(AppearAnimation());
	}

	IEnumerator AppearAnimation()
	{
		transform.position = GetRandomValidPos();
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
		StartCoroutine(DisappearAnimation());
	}

	private IEnumerator DisappearAnimation()
	{
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
		StartCoroutine(WaitCooldownToAttack());
	}

	IEnumerator WaitCooldownToAttack()
	{
		yield return new WaitForSeconds(attackCooldown);
		TripleBombAttack bombCaster = Instantiate(this.bombCaster);
		bombCaster.SetTarget(player);
		bombCaster.transform.position = this.transform.position;
		bombCaster.transform.parent = this.transform;
	}

	public override IAttack ChooseOneAttack()
	{
		int attackIndex = UnityEngine.Random.Range(0, myAttacks.Count);
		return myAttacks[attackIndex];
	}

	public override void Die()
	{
		throw new System.NotImplementedException();
	}

	public override void Move()
	{
		throw new System.NotImplementedException();
	}
}

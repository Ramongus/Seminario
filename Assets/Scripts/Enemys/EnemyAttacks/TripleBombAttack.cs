using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleBombAttack : MonoBehaviour, IAttack
{
	[SerializeField] GameObject singleBombPrefab;
	[SerializeField] GameObject explodeAreaMarker;
	[SerializeField] float betweenBombsCooldown;
	[SerializeField] float timeOfBombFly;
	Rigidbody bombRifi;

	[SerializeField] Transform castTransform;
	[SerializeField] Transform target;

	[SerializeField] float minAngleVariation;
	[SerializeField] float maxAngleVariation;
	[SerializeField] float minDistanceVariation;
	[SerializeField] float maxDistanceVariation;

	/*
	public TripleBombAttack(GameObject singleBombPrefab, GameObject explodeAreaMarker, Transform from, Transform to, float minAngleVariation, float maxAngleVariation, float minDistanceVariation, float maxDistanceVariation)
	{
		this.singleBombPrefab = singleBombPrefab;
		this.explodeAreaMarker = explodeAreaMarker;
		bombRifi = this.singleBombPrefab.GetComponent<Rigidbody>();

		startPos = from;
		target = to;

		this.minAngleVariation = minAngleVariation;
		this.maxAngleVariation = maxAngleVariation;
		this.minDistanceVariation = minDistanceVariation;
		this.maxDistanceVariation = maxDistanceVariation;
	}
	*/

	private void Start()
	{
		Attack();
	}

	public void Attack()
	{
		StartCoroutine(BombsBurst());
	}

	private IEnumerator BombsBurst()
	{
		for (int i = 0; i < 3; i++)//Fijado en 3 pero se puede hacer que no sea solo triple
		{
			CreateBomb();
			yield return new WaitForSeconds(betweenBombsCooldown);
		}
		Destroy(this.gameObject);
	}

	private void CreateBomb()
	{
		GameObject oneBomb = Instantiate(singleBombPrefab);
		oneBomb.transform.position = castTransform.position;
		Rigidbody rigi = oneBomb.GetComponent<Rigidbody>();
		Vector3 initialVelocity = GetInitialVelocity();
		rigi.AddForce(initialVelocity, ForceMode.Impulse);
	}

	private Vector3 GetInitialVelocity()
	{
		//Vector3 deltaPos = target.position - castTransform.position;

		//Tiro oblicuo
		//x - en realidad mas que en x es en el plano xz

		/*
		Vector3 planeDir = deltaPos.normalized;
		float distance = deltaPos.magnitude;
		*/
		Vector3 planeDir = (target.position - castTransform.position).normalized;
		float distance = (target.position - castTransform.position).magnitude;
		Vector3 velInPlane = planeDir * distance / timeOfBombFly;

		//y
		/*
		Debug.Log("Gravity: " + Physics.gravity.y);
		float deltaY = target.position.y - castTransform.transform.position.y;
		float velYMagnitud = (deltaY - ((1 / 2) * Physics.gravity.y * Mathf.Pow(timeOfBombFly, 2))) / timeOfBombFly;
		Vector3 velUp = Vector3.up * velYMagnitud;
		*/

		Vector3 velUp = -Physics.gravity.y * timeOfBombFly / 2 * Vector3.up;
		return velInPlane + velUp;
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}
}

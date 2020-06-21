using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleBombAttack : MonoBehaviour, IAttack
{
	[SerializeField] Bomb singleBombPrefab;
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

	[SerializeField] GameObject explosionFieldPrefab;

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

	private void Awake()
	{
		target = FindObjectOfType<Player>().transform;
	}

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
		Bomb oneBomb = Instantiate(singleBombPrefab);
		oneBomb.transform.position = castTransform.position;

		float randDistVariation = UnityEngine.Random.Range(minDistanceVariation, maxDistanceVariation);
		Quaternion randAngleVariation = Quaternion.Euler(0, UnityEngine.Random.Range(minAngleVariation, maxAngleVariation), 0);
		Vector3 predictPosition = target.position + randAngleVariation * target.forward * randDistVariation;
		Vector3 fallingBombPoint = predictPosition;
		GameObject explosionField = Instantiate(explosionFieldPrefab);

		oneBomb.SetExplodeField(explosionField);

		explosionField.transform.position = fallingBombPoint;
		float explodeDiameter = oneBomb.explodeRadius * 2;
		explosionField.transform.localScale = new Vector3(explodeDiameter, explodeDiameter, explodeDiameter);

		Rigidbody rigi = oneBomb.GetComponent<Rigidbody>();
		Vector3 initialVelocity = GetInitialVelocity(predictPosition);
		rigi.AddForce(initialVelocity, ForceMode.Impulse);
	}

	private Vector3 GetInitialVelocity(Vector3 finalTargetPos)
	{
		Vector3 planeDir = finalTargetPos - castTransform.position;
		Vector3 velInPlane = planeDir / timeOfBombFly;

		Vector3 velUp = -Physics.gravity.y * timeOfBombFly / 2 * Vector3.up;
		return velInPlane + velUp;
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}
}

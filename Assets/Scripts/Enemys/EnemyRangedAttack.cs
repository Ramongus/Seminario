using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyRangedAttack : MonoBehaviour
{
	[Header("Ranged Attack Properties")]
	[SerializeField] protected float damage;

	public abstract void PositionAttack(GameObject owner, GameObject objective);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyWrapper
{
	[Header("Enemy Ranged Properties")]
	[SerializeField] float attackRange;
	[SerializeField] GameObject objective;
	[SerializeField] float attackCooldown;

	public bool IsOnObjectiveOnRange()
	{
		return Vector3.Distance(this.transform.position, objective.transform.position) <= attackRange;
	}

	public virtual void RePosition()
	{

	}
}

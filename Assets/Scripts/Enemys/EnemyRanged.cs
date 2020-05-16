using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyRanged : EnemyWrapper
{
	[Header("Enemy Ranged Properties")]
	[SerializeField] protected float attackRange;
	[SerializeField] protected GameObject objective;
	[SerializeField] protected float attackCooldown;
	[SerializeField] protected EnemyRangedAttack attackPrefab;

	public abstract void RePosition();

	public virtual bool IsObjectiveOnRange()
	{
		return Vector3.Distance(this.transform.position, objective.transform.position) <= attackRange;
	}

	public virtual void Attack()
	{
		EnemyRangedAttack attack = Instantiate(attackPrefab);
		attack.PositionAttack(this.gameObject, objective);
	}
}


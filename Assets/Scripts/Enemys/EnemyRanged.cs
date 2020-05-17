using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyRanged : EnemyWrapper
{
	[Header("Enemy Ranged Properties")]
	[SerializeField] protected float attackRange;
	[SerializeField] public GameObject objective;
	[SerializeField] protected float attackCooldown;
	[SerializeField] protected EnemyRangedAttack attackPrefab;
	[SerializeField] public Animator animator;
	[SerializeField] public float rotationSpeed;
	[SerializeField] protected float speed;
	[SerializeField] protected float angleOfSight;

	public abstract void RePosition();
	public abstract bool CheckAttackConditions();

	public virtual void Attack()
	{
		EnemyRangedAttack attack = Instantiate(attackPrefab);
		attack.PositionAttack(this.gameObject, objective);
	}

	public void SetVelocity(Vector3 velocity)
	{
		GetComponent<Rigidbody>().velocity = velocity * speed;
	}
}

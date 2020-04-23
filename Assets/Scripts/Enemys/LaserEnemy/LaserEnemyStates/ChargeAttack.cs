using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : State
{
	IChargeAttack attack;
	Transform owner;
	Transform objective;
	Transform castAttackPoint;
	LaserEnemy laserInstantiator;

	public ChargeAttack(StateMachine sm, IChargeAttack attack, Transform owner, Transform objective, Transform castAttackPoint, LaserEnemy laserInstantiator) : base(sm)
	{
		this.attack = attack;
		this.owner = owner;
		this.objective = objective;
		this.castAttackPoint = castAttackPoint;
		this.laserInstantiator = laserInstantiator;
	}

	public override void Awake()
	{
		base.Awake();
		laserInstantiator.InstantiateLaser(castAttackPoint.position);
		Debug.Log("Charging");
	}

	public override void Execute()
	{
		if (!IsInPosition())
		{
			_sm.SetState<MoveNearObjective>();
		}

		if (attack.Charge())
		{
			Debug.Log("Attack");
			attack.Attack();
			//Aca deberia ir a un nuevo estado de "Recuperacion de mana" o algo asi en el que se mueva por los alrededores;
		}
	}

	private bool IsInPosition()
	{
		Vector3 ignoreObjectiveHeight = new Vector3(objective.position.x, castAttackPoint.position.y, objective.position.z);
		owner.forward = Vector3.Normalize(ignoreObjectiveHeight - owner.position);

		RaycastHit hit;
		if (Physics.Raycast(owner.position, Vector3.Normalize(ignoreObjectiveHeight - owner.position), out hit, 1000))
		{
			Player player = hit.collider.GetComponent<Player>();
			if(player == null)
			{
				Debug.Log("Se esta interponiendo entre el raycast y el player: " + hit.transform.name);
			}
			laserInstantiator.UpdateLaser(Vector3.zero, ignoreObjectiveHeight - castAttackPoint.position);
			return player != null;
		}
		attack.ResetCharge();
		return false;
	}
}

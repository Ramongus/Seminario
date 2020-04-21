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

	public ChargeAttack(StateMachine sm, IChargeAttack attack, Transform owner, Transform objective, Transform castAttackPoint) : base(sm)
	{
		this.attack = attack;
		this.owner = owner;
		this.objective = objective;
		this.castAttackPoint = castAttackPoint;
	}

	public override void Awake()
	{
		base.Awake();
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
			attack.Attack();
			//Aca deberia ir a un nuevo estado de "Recuperacion de mana" o algo asi en el que se mueva por los alrededores;
		}
	}

	private bool IsInPosition()
	{
		RaycastHit hit = new RaycastHit();
		Debug.Log(castAttackPoint);
		Debug.Log(objective);
		if (Physics.Raycast(castAttackPoint.position, Vector3.Normalize(objective.position - castAttackPoint.position), out hit))
		{
			return hit.collider.GetComponent<Player>() != null;
		}
		attack.ResetCharge();
		return false;
	}
}

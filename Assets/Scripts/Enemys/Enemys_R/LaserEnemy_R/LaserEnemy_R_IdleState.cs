using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy_R_IdleState : State
{
	LaserEnemy_R owner;

	public LaserEnemy_R_IdleState(StateMachineClassic sm, LaserEnemy_R owner) : base(sm)
	{
		this.owner = owner;
	}

	public override void Awake()
	{
		base.Awake();
		owner.SetIdleAnimation();
	}
}

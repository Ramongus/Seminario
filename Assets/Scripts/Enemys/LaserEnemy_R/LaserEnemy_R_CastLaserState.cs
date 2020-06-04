using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy_R_CastLaserState : State
{
	LaserEnemy_R owner;
	float timer;

	public LaserEnemy_R_CastLaserState(StateMachineClassic sm, LaserEnemy_R owner) : base(sm)
	{
		this.owner = owner;
	}

	public override void Awake()
	{
		base.Awake();
		owner.SetCastingAnimation();
		timer = owner.castingTime;
	}

	public override void Execute()
	{
		base.Execute();
		timer -= Time.deltaTime;
		if (timer <= 0) owner.Attack();
	}
}

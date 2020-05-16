using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWrapper : MonoBehaviour
{
	EnemyModel _model;
	EnemyView _view;

	private void Awake()
	{
		_model = new EnemyModel();
		_view = new EnemyView();
	}

	private void Update()
	{
		_model.UpdateModel();
	}
}

public class EnemyModel
{
	StateMachineClassic _sm;
	public EnemyModel()
	{
		_sm = new StateMachineClassic();
		_sm.AddState(new ChangeToStateAfterSeconds<EmptyStateTest>(_sm, 10));
		_sm.AddState(new EmptyStateTest(_sm));
	}

	public void UpdateModel()
	{
		_sm.Update();
	}
}

public class EnemyView
{

}

public class EmptyStateTest : State
{
	public EmptyStateTest(StateMachineClassic sm) : base(sm)
	{
	}

	public override void Awake()
	{
		base.Awake();
		Debug.Log("EmptyState");
	}
}

public class ChangeToStateAfterSeconds<T> : State where T : State
{
	float secondsCharging;

	float timer;

	public ChangeToStateAfterSeconds(StateMachineClassic sm, float timeCharging) : base(sm)
	{
		secondsCharging = timeCharging;
		timer = secondsCharging;
	}

	public override void Awake()
	{
		timer = secondsCharging;
	}

	public override void Execute()
	{
		timer -= Time.deltaTime;
		Debug.Log("Timer: " + timer);
		if (timer <= 0)
			_sm.SetState<T>();
	}
}

public class MetheoriteAttackState : State
{


	public MetheoriteAttackState(StateMachineClassic sm) : base(sm)
	{
	}

	public override void Awake()
	{
		base.Awake();
		CreateBombs();
	}

	private void CreateBombs()
	{
		
	}
}

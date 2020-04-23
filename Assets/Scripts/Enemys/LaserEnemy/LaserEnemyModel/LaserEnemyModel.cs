using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemyModel
{
	LaserEnemyView _view;

	StateMachine _stateMachine;

	public LaserEnemyModel(LaserEnemyView view, Transform owner, List<ANode> availableNodes, float speed, List<Transform> patrolWaypoints, bool patrolLoop, Transform objective, float distArroundPlayer, Laser attack, Transform castPoint, LaserEnemy entity)
	{
		_view = view;
		_stateMachine = new StateMachine();
		_stateMachine.AddState(new PatrolState(_stateMachine, owner, speed, patrolWaypoints, patrolLoop));
		_stateMachine.AddState(new MoveNearObjective(_stateMachine, availableNodes, owner, objective, distArroundPlayer, speed));
		_stateMachine.AddState(new ChargeAttack(_stateMachine, attack, owner, objective, castPoint, entity));
	}

	public void UpdateStateMachine()
	{
		_stateMachine.Update();
	}

	public void ChangeState<T>() where T:State
	{
		_stateMachine.SetState<T>();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemyModel
{
	LaserEnemyView _view;

	StateMachine _stateMachine;

	public LaserEnemyModel(LaserEnemyView view, Transform owner, float speed, List<Transform> patrolWaypoints, bool patrolLoop)
	{
		_view = view;
		_stateMachine = new StateMachine();
		_stateMachine.AddState(new PatrolState(_stateMachine, owner, speed, patrolWaypoints, patrolLoop));
	}

	public void UpdateStateMachine()
	{
		_stateMachine.Update();
	}
}

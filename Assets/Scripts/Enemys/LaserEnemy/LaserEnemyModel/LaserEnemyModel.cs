using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemyModel : MonoBehaviour
{
	LaserEnemyView _view;

	StateMachine _stateMachine;

	public LaserEnemyModel(LaserEnemyView view)
	{
		_view = view;
		_stateMachine = new StateMachine();
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
	[SerializeField] List<Transform> patrolWaypoints;
	[SerializeField] float speed;
	[SerializeField] bool patrolStateLoop;

	LaserEnemyView _view;
	LaserEnemyModel _model;
	LaserEnemyController _controller;

	private void Awake()
	{
		_view = new LaserEnemyView();
		_model = new LaserEnemyModel(_view, transform, speed, patrolWaypoints, patrolStateLoop);
		_controller = new LaserEnemyController(_model);
	}

	private void Update()
	{
		_model.UpdateStateMachine();
	}
}

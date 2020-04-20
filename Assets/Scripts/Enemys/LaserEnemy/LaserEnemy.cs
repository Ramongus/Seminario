using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
	LaserEnemyView _view;
	LaserEnemyModel _model;
	LaserEnemyController _controller;

	private void Awake()
	{
		_view = new LaserEnemyView();
		_model = new LaserEnemyModel(_view);
		_controller = new LaserEnemyController(_model);
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
	[SerializeField] Laser laserAttack;
	[SerializeField] Transform castPoint;
	[SerializeField] List<Transform> patrolWaypoints;
	[SerializeField] float speed;
	[SerializeField] bool patrolStateLoop;
	[SerializeField] float distanceToPlayer;

	LaserEnemyView _view;
	LaserEnemyModel _model;
	LaserEnemyController _controller;

	private void Awake()
	{
		_view = new LaserEnemyView();
		_model = new LaserEnemyModel(_view, transform, FindObjectOfType<NodesList>().GetNodes(), speed, patrolWaypoints, patrolStateLoop, FindObjectOfType<Player>().transform, distanceToPlayer, laserAttack, castPoint);
		_controller = new LaserEnemyController(_model);
	}

	private void Update()
	{
		_model.UpdateStateMachine();
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("ENEMY IA DETECT PLAYER");
		Player player = other.GetComponent<Player>();
		if(player != null)
		{
			ForceChangeState<MoveNearObjective>();
		}
	}

	private void ForceChangeState<T>() where T:State
	{
		_model.ChangeState<T>();
	}
}

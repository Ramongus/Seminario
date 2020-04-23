using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : MonoBehaviour, IUpdate
{
	[SerializeField] LineRenderer laserRender;
	[SerializeField] Laser laserAttack;
	[SerializeField] Transform castPoint;
	[SerializeField] List<Transform> patrolWaypoints;
	[SerializeField] float speed;
	[SerializeField] bool patrolStateLoop;
	[SerializeField] float distanceToPlayer;

	LineRenderer currentLaser;

	LaserEnemyView _view;
	LaserEnemyModel _model;
	LaserEnemyController _controller;

	private void Awake()
	{
		EventsManager.TriggerEvent("SuscribeToUpdateManager", this);
		_view = new LaserEnemyView();
		_model = new LaserEnemyModel(_view, transform, FindObjectOfType<NodesList>().GetNodes(), speed, patrolWaypoints, patrolStateLoop, FindObjectOfType<Player>().transform, distanceToPlayer, laserAttack, castPoint, this);
		_controller = new LaserEnemyController(_model);
	}

	public void MyUpdate()
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
			GetComponent<Collider>().enabled = false;
		}
	}

	private void ForceChangeState<T>() where T:State
	{
		_model.ChangeState<T>();
	}

	public void InstantiateLaser(Vector3 position)
	{
		currentLaser = Instantiate(laserRender);
		currentLaser.transform.position = position;
	}

	public void UpdateLaser(Vector3 pos1, Vector3 pos2)
	{
		currentLaser.SetPosition(0, pos1);
		currentLaser.SetPosition(1, pos2);
	}
}

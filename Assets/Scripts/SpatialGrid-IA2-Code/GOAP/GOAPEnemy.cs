using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;
using System.Linq;

public class GOAPEnemy : MonoBehaviour
{
	FiniteStateMachine _fsm;
	public PatrolState patrolState;
	public ChaseState chaseState;
	public MeleeAttackState meleeAttackState;
	public RangeAttackState rangeAttackState;
	public ChargeManaState chargeManaState;
	private GoapPlanner planner;
	public GOAPState from;
	private GOAPState to;
	private IEnumerable<GOAPAction> actions;
	Player player;
	public float distanceToChooseMelee;

	float sqrDistance;
	float dist;
	Vector3 dir;

	[Header("Player In Sight Values")]
	public LayerMask obstacleLayer;
	public float sightRange;
	[Header("Player In Range Values")]
	public float range;
	[Header("Player Near Values")]
	public float nearDistance;
	[Header("Mana Values")]
	public float maxMana;
	public float mana;

	private void Awake()
	{
		EventsManager.SuscribeToEvent("RePlan", RePlan);
		player = FindObjectOfType<Player>();
	}

	//Update que actualice el estado de nuestras variables constantemente
	// y cuando algun estado cambia realizar un re-planiamiento.

	private void Start()
	{
		PlanAndExecute();
	}

	private void Update()
	{
		var playerPosAtMyHeight = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
		var delta = (playerPosAtMyHeight - transform.position);
		sqrDistance = delta.sqrMagnitude;
		dist = delta.magnitude;
		dir = delta.normalized;


		bool replan = false;

		bool playerOnSight = IsOnSight();
		if(from.values["isPlayerInSight"] != playerOnSight)
		{
			from.values["isPlayerInSight"] = playerOnSight;
			replan = true;
		}

		bool playerInRange = IsOnRange();
		if(from.values["isPlayerInRange"] != playerOnSight)
		{
			from.values["isPlayerInRange"] = playerOnSight;
			replan = true;
		}

		bool playerNear = IsNear();
		if(from.values["isPlayerNear"] != playerNear)
		{
			from.values["isPlayerNear"] = playerNear;
			replan = true;
		}

		bool hasFullMana = IsFullMana();
		if(from.values["hasMana"] != hasFullMana)
		{
			from.values["hasMana"] = hasFullMana;
			replan = true;
		}

		if (replan)
		{
			EventsManager.TriggerEvent("RePlan", this);
		}
	}

	private bool IsFullMana()
	{
		return mana == maxMana;
	}

	private bool IsNear()
	{
		return dist < nearDistance;
	}

	private bool IsOnRange()
	{
		return dist < range;
	}

	private bool IsOnSight()
	{
		bool isOnSight = true;
		RaycastHit hit;
		Physics.Raycast(transform.position, dir, out hit, dist, obstacleLayer);
		if (hit.collider != null)
		{
			isOnSight = false;
		}

		if (sqrDistance < sightRange * sightRange && isOnSight)
		{
			return true;
		}
		return false;
	}

	private void RePlan(object[] parameters)
	{
		if((GOAPEnemy)parameters[0] == this)
		{
			//Tendriamos que hacer aca el seteo del estado dependiendo las condiciones????
			StopAllCoroutines();
			var nActions = new List<GOAPAction>();

			foreach (var item in actions)
			{
				if (item.name == "Charge Mana")
					nActions.Add(item.Cost(2 - mana / maxMana));
				else if (item.name == "Melee Attack")
					nActions.Add(item.Cost(1));
				else
					nActions.Add(item);
			}
			actions = nActions;
			planner.Run(from, to, actions, StartCoroutine);
			_fsm = GoapPlanner.ConfigureFSM(actions, StartCoroutine);
		}
	}

	private void PlanAndExecute()
	{
		actions = new List<GOAPAction>{
											  new GOAPAction("Patrol")
												.Pre("hasMana", true)
												 .Effect("isPlayerInSight", true)
												 .LinkedState(patrolState)
												 .Cost(2),

											  new GOAPAction("Chase")
												 .Pre("isPlayerInSight", true)
												 .Effect("isPlayerNear",    true)
												 .Effect("isPlayerInRange", true)
												 .LinkedState(chaseState),

											  new GOAPAction("Melee Attack")
												 .Pre("isPlayerNear",   true)
												 .Effect("isPlayerAlive", false)
												 .LinkedState(meleeAttackState)
												 .Cost(3),

											  new GOAPAction("Range Attack")
												.Pre("isPlayerInRange", true)
												.Pre("hasMana", true)
												.Effect("isPlayerAlive", false)
												.LinkedState(rangeAttackState),

											  new GOAPAction("Charge Mana")
												.Effect("hasMana", true)
												.LinkedState(chargeManaState)
										  };
		from = new GOAPState();
		from.values["isPlayerInSight"] = false;
		from.values["isPlayerNear"] = false;
		from.values["isPlayerInRange"] = false;
		from.values["hasMana"] = false;
		from.values["isPlayerAlive"] = true;
		to = new GOAPState();
		to.values["isPlayerAlive"] = false;

		planner = new GoapPlanner();
		planner.OnPlanCompleted += OnPlanCompleted;
		planner.OnCantPlan += OnCantPlan;

		planner.Run(from, to, actions, StartCoroutine);
	}


	private void OnPlanCompleted(IEnumerable<GOAPAction> plan)
	{
		_fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
		_fsm.Active = true;
	}

	private void OnCantPlan()
	{
		Debug.LogWarning("!!! NO PUDO COMPLETAR EL PLAN !!!");

		actions = new List<GOAPAction>{
											  new GOAPAction("Patrol")
												 .Effect("isPlayerInSight", true)
												 .LinkedState(patrolState)
												 .Cost(2),

											  new GOAPAction("Chase")
												 .Pre("isPlayerInSight", true)
												 .Effect("isPlayerNear",    true)
												 .Effect("isPlayerInRange", true)
												 .LinkedState(chaseState),

											  new GOAPAction("Melee Attack")
												 .Pre("isPlayerNear",   true)
												 .Effect("isPlayerAlive", false)
												 .LinkedState(meleeAttackState),

											  new GOAPAction("Range Attack")
												.Pre("isPlayerInRange", true)
												.Pre("hasMana", true)
												.Effect("isPlayerAlive", false)
												.Effect("hasMana", false)
												.LinkedState(rangeAttackState),

											  new GOAPAction("Charge Mana")
												.Effect("hasMana", true)
												.LinkedState(chargeManaState)
										  };
		from = new GOAPState();
		from.values["isPlayerInSight"] = false;
		from.values["isPlayerNear"] = false;
		from.values["isPlayerInRange"] = false;
		from.values["hasMana"] = false;
		from.values["isPlayerAlive"] = true;
		to = new GOAPState();
		to.values["isPlayerAlive"] = false;

		planner = new GoapPlanner();
		planner.OnPlanCompleted += OnPlanCompleted;
		planner.OnCantPlan += OnCantPlan;

		planner.Run(from, to, actions, StartCoroutine);

		_fsm = GoapPlanner.ConfigureFSM(actions, StartCoroutine);
	}
}

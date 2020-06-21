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
	public float maxMana;
	public float mana;
	Player player;
	public float distanceToChooseMelee;

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

	private void RePlan(object[] parameters)
	{
		if((GOAPEnemy)parameters[0] == this)
		{
			//Tendriamos que hacer aca el seteo del estado dependiendo las condiciones????
			StopAllCoroutines();
			//actions = actions.Where(x => x.name == "Melee Attack").Select(x => x.Cost(1.5f + mana / maxMana));
			planner.Run(from, to, actions, StartCoroutine);
		}
	}

	private void PlanAndExecute()
	{
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
												 .LinkedState(meleeAttackState)
												 .Cost(3),

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
		/*
		from.values["isPlayerInSight"] = false;
		from.values["isPlayerNear"] = false;
		from.values["isPlayerInRange"] = false;
		from.values["hasMana"] = false;
		*/
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

		var actions = new List<GOAPAction>
										{
											new GOAPAction("Patrol")
												 .Effect("isPlayerInSight", true)
												 .LinkedState(patrolState)
										};

		var from = new GOAPState();
		from.values["isPlayerInSight"] = false;

		var to = new GOAPState();
		to.values["isPlayerInSight"] = true;

		var planner = new GoapPlanner();
		planner.OnPlanCompleted += OnPlanCompleted;
		planner.OnCantPlan += OnCantPlan;

		planner.Run(from, to, actions, StartCoroutine);

		_fsm = GoapPlanner.ConfigureFSM(actions, StartCoroutine);
	}
}

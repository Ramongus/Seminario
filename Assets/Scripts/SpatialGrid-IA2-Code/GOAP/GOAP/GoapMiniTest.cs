using System.Collections.Generic;
using FSM;
using UnityEngine;

public class GoapMiniTest : MonoBehaviour {

    public PatrolState      patrolState;
    public ChaseState       chaseState;
    public MeleeAttackState meleeAttackState;

    private FiniteStateMachine _fsm;
    
    
    void Start() {
        //OnlyPlan();
        PlanAndExecute();
    }

    private void OnlyPlan() {
        var actions = new List<GOAPAction>{
                                              new GOAPAction("Patrol")
                                                 .Effect("isPlayerInSight", true),

                                              new GOAPAction("Chase")
                                                 .Pre("isPlayerInSight", true)
                                                 .Effect("isPlayerInRange", true)
                                                 .Effect("isPlayerNear",    true),

                                              new GOAPAction("Melee Attack")
                                                 .Pre("isPlayerNear",   true)
                                                 .Pre("hasMeleeWeapon", true)
                                                 .Effect("isPlayerAlive", false)
                                                 .Cost(2f),

                                              new GOAPAction("Range Attack")
                                                 .Pre("isPlayerInRange", true)
                                                 .Pre("hasRangeWeapon",  true)
                                                 .Effect("isPlayerAlive", false),

                                              new GOAPAction("Pick Melee Weapon")
                                                 .Effect("hasMeleeWeapon", true)
                                                 .Cost(2f),

                                              new GOAPAction("Pick Range Weapon")
                                                 .Effect("hasRangeWeapon", true)
                                          };
        var from = new GOAPState();
        from.values["isPlayerInSight"] = true;
        from.values["isPlayerNear"]    = true;
        from.values["isPlayerInRange"] = true;
        from.values["isPlayerAlive"]   = true;
        from.values["hasRangeWeapon"]  = true;
        from.values["hasMeleeWeapon"]  = false;

        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;

        var planner = new GoapPlanner();
        //planner.OnPlanCompleted += OnPlanCompleted;
        //planner.OnCantPlan      += OnCantPlan;

        planner.Run(from, to, actions, StartCoroutine);
    }

    private void PlanAndExecute() {
        var actions = new List<GOAPAction>{
                                              new GOAPAction("Patrol")
                                                 .Effect("isPlayerInSight", true)
                                                 .LinkedState(patrolState),

                                              new GOAPAction("Chase")
                                                 .Pre("isPlayerInSight", true)
                                                 .Effect("isPlayerNear",    true)
                                                 .LinkedState(chaseState),

                                              new GOAPAction("Melee Attack")
                                                 .Pre("isPlayerNear",   true)
                                                 .Effect("isPlayerAlive", false)
                                                 .LinkedState(meleeAttackState)
                                          };
        
        var from = new GOAPState();
        from.values["isPlayerInSight"] = false;
        from.values["isPlayerNear"]    = false;
        from.values["isPlayerAlive"]   = true;

        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;

        var planner = new GoapPlanner();
        planner.OnPlanCompleted += OnPlanCompleted;
        planner.OnCantPlan      += OnCantPlan;

        planner.Run(from, to, actions, StartCoroutine);
    }


    private void OnPlanCompleted(IEnumerable<GOAPAction> plan) {
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

    private void OnCantPlan() {
        //TODO: debuggeamos para ver por qué no pudo planear y encontrar como hacer para que no pase nunca mas
    }

}

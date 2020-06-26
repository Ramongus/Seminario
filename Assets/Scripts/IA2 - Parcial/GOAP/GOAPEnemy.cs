using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;
using System.Linq;
using UnityEngine.UI;

//IA2-P2
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


    public Image manaImage;
    public float rotSpeed;
    
    float sqrDistance;
    float dist;
    Vector3 dir;

    [Header("Player In Sight Values")] public LayerMask obstacleLayer;
    public float sightRange;
    [Header("Player In Range Values")] public float range;
    [Header("Player Near Values")] public float nearDistance;
    [Header("Mana Values")] public float maxMana;
    public float mana;

    public bool justReplaned;
    private void Awake()
    {
        EventsManager.SuscribeToEvent("RePlan", RePlan);
        player = FindObjectOfType<Player>();
        manaImage.fillAmount = maxMana / 50;
    }

    //Update que actualice el estado de nuestras variables constantemente
    // y cuando algun estado cambia realizar un re-planiamiento.

    private void Start()
    {
        PlanAndExecute();
    }

    private void Update()
    {
        /*
         * ¿Por qué hicimos to-do esto?
         * Nosotros segun la clase teorica, entendimos que el enemigo debia poder pasar de un estado a otro,
         * y que los estados no definan las transiciones entre ellos, ya que esto nos resultaba igual a la clasica FSM.
         * Por ello, hicimos esto: (muy desprolijo pero intentando seguir la teoria).
         *     1_ Calcular todas las condiciones que significan un cambio en el estado de nuestro enemigo.
         *     2_ Si nuestro estado cambió, entonces nos fijamos si en nuestro plan hay algun estado que cumpla con sus pre-condiciones.
         *     3_ Si no encuentra ningun estado dentro del plan que cumpla con sus precondiciones, entonces re-planea.
         *
         *     ACLARACIONES:
         *         Lo que esta bueno de esto, es que cuando busca un estado a cambiar dentro de su plan, lo hace de atras para adelante.
         *         Por ejemplo, si estaba patrullando y en el mismo frame lo vé al player y a su vez lo tiene en rango de ataque, no
         *      será necesario que el enemigo pase por el estado "chase" para llegar al estado "attack".
         *         Básicamente pasa al estado que lo hace llegar a su objetivo más rápido, sin la necesidad de pasar por estados innecesarios.
         *
         *     LO MALO:
         *         Lo malo, es que por una cuestión de tiempos, no pudimos implementar desde cero GOAP. Esto nos llevo a tener que hacer
         *      conversiones de tipos de datos de más para llegar a un objetivo.
         *         Esto desembocó en un código dificil de entender.
         */
        
        var playerPosAtMyHeight = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        var delta = (playerPosAtMyHeight - transform.position);
        sqrDistance = delta.sqrMagnitude;
        dist = delta.magnitude;
        dir = delta.normalized;

        bool stateHasChange = false;

        stateHasChange = CheckIfMyStateChange();

        if(!stateHasChange) return;
        
        if (CheckIfNextStateIsInPlan()) return;
        Debug.LogWarning("RE planeo");
        justReplaned = true;
        EventsManager.TriggerEvent("RePlan", this);
    }

    private bool CheckIfMyStateChange()
    {
        bool stateHasChange = false;
        
        bool playerOnSight = IsOnSight();
        if (@from.values["isPlayerInSight"] != playerOnSight)
        {
            @from.values["isPlayerInSight"] = playerOnSight;
            stateHasChange = true;
        }

        bool playerInRange = IsOnRange();
        if (@from.values["isPlayerInRange"] != playerOnSight)
        {
            @from.values["isPlayerInRange"] = playerOnSight;
            stateHasChange = true;
        }

        bool playerNear = IsNear();
        if (@from.values["isPlayerNear"] != playerNear)
        {
            @from.values["isPlayerNear"] = playerNear;
            stateHasChange = true;
        }

        bool hasFullMana = IsFullMana();
        if (@from.values["hasMana"] != hasFullMana)
        {
            @from.values["hasMana"] = hasFullMana;
            stateHasChange = true;
        }

        return stateHasChange;
    }

    private bool CheckIfNextStateIsInPlan()
    {
        //IA2-P3
        var myPlanGOAPStates = planner.GetPlan();
        var myPlanStates = planner.GetPlan().Select(x => x.generatingAction.linkedState);
        if (myPlanStates.Any())
        {
            var nextStatesInPlan = myPlanStates.Aggregate(new List<IState>(), (acum, state) =>
                {
                    //Dentro de mi plan....
                    if (acum.Count == 0) //Mientras que mi acumulador siga vacio
                    {
                        if (state == _fsm.CurrentState) //Cuando encuentre mi actual estado
                            acum.Add(state); //Lo agrego para que no entre mas al if.
                    }
                    else
                    {
                        acum.Add(state); //Agrego todos los siguientes a mi actual
                    }

                    return acum; //Devuelvo todos los siguientes estados.
                })
                .Skip(1); //Menos el estado actual.
            List<GOAPState> fromStateToGOAPStates = new List<GOAPState>();
            foreach (var state in nextStatesInPlan)
            {
                foreach (var goapState in myPlanGOAPStates)
                {
                    if (goapState.generatingAction.linkedState == state)
                    {
                        fromStateToGOAPStates.Add(goapState);
                        break;
                    }
                }
            }

            var nextState = fromStateToGOAPStates.Aggregate(new GOAPState(), (selectedState, elem) =>
            {
                Dictionary<string, bool> keyValue = elem.generatingAction.preconditions;
                bool canGoToState = true;
                foreach (var pair in keyValue)
                {
                    if (!@from.values[pair.Key] == pair.Value) canGoToState = false;
                }

                if (canGoToState)
                    return elem;
                if (selectedState != null)
                    return selectedState;
                return null;
            });
            if (nextState.generatingAction != null)
            {
                _fsm.CurrentState = nextState.generatingAction.linkedState;
                Debug.LogWarning("Cambie a un estado que ya estaba en mi plan ;)");
                return true;
            }
        }
        return false;
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
        if ((GOAPEnemy) parameters[0] == this)
        {
            //Tendriamos que hacer aca el seteo del estado dependiendo las condiciones????
            StopAllCoroutines();
            //IA2-P3
            var nIEActions = actions.Aggregate(new List<GOAPAction>(), (acum, action) =>
            {
                //Intento de "Costos dinamicos"
                if (action.name == "Charge Mana")
                    acum.Add(action.Cost(2 - mana / maxMana));
                else if (action.name == "Melee Attack")
                {
                    var playerPosAtMyHeight = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                    var delta = (playerPosAtMyHeight - transform.position);
                    acum.Add(action.Cost(1 + Mathf.Clamp01(delta.magnitude / distanceToChooseMelee)));    
                }
                else
                    acum.Add(action);

                return acum;
            });

            actions = nIEActions;
            planner.Run(from, to, actions, StartCoroutine);
            _fsm = GoapPlanner.ConfigureFSM(actions, StartCoroutine);
        }
    }

    private void PlanAndExecute()
    { 
        actions = new List<GOAPAction>
        {
            new GOAPAction("Patrol")
                .Pre("hasMana", true)
                .Effect("isPlayerInSight", true)
                .LinkedState(patrolState)
                .Cost(2),

            new GOAPAction("Chase")
                .Pre("isPlayerInSight", true)
                .Effect("isPlayerNear", true)
                .Effect("isPlayerInRange", true)
                .LinkedState(chaseState),

            new GOAPAction("Melee Attack")
                .Pre("isPlayerNear", true)
                .Effect("isPlayerAlive", false)
                .LinkedState(meleeAttackState)
                .Cost(2),

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

        actions = new List<GOAPAction>
        {
            new GOAPAction("Patrol")
                .Effect("isPlayerInSight", true)
                .LinkedState(patrolState)
                .Cost(2),

            new GOAPAction("Chase")
                .Pre("isPlayerInSight", true)
                .Effect("isPlayerNear", true)
                .Effect("isPlayerInRange", true)
                .LinkedState(chaseState),

            new GOAPAction("Melee Attack")
                .Pre("isPlayerNear", true)
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
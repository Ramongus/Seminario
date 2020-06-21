using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedMetheoriteSpawner : EnemyRanged
{
	[Header("Enemy Metheorite Properties")]
	[SerializeField] string enemyName;
	[SerializeField] LayerMask obstaclesAndPlayerLayer;
	StateMachineClassic _sm;
	float attackTimer;

	private void Start()
	{
		_sm = new StateMachineClassic();
		_sm.AddState(new EnemyRangedMetheoriteSpawner_AttackState(_sm, this));
		_sm.AddState(new EnemyRangedMetheoriteSpawner_SpawnState(_sm, this));
		_sm.AddState(new EnemyRangedMetheoriteSpawner_CooldownState(_sm, this));
		_sm.AddState(new EnemyRangedMetheoriteSpawner_PathfindingState(_sm, this, FindObjectOfType<NodesList>().GetNodes()));
		_sm.SetState<EnemyRangedMetheoriteSpawner_SpawnState>();
		attackTimer = 0;
	}

	private void Update()
	{
		attackTimer -= Time.deltaTime;
		_sm.Update();
	}

	public void MakeAttack()
	{
		if (_sm.IsActualState<EnemyRangedMetheoriteSpawner_AttackState>()) return;

		SetVelocity(transform.forward * 0);
		_sm.SetState<EnemyRangedMetheoriteSpawner_AttackState>();
	}

	public override void Attack()
	{
		base.Attack();
		Debug.Log("Ataca!");
		attackTimer = attackCooldown;
	}

	public void FinishAttack()
	{
		Debug.Log("Deja de atacar.");
		animator.SetBool("Attack", false);
		_sm.SetState<EnemyRangedMetheoriteSpawner_CooldownState>();
	}

	public void StartAttackAnimation()
	{
		animator.SetBool("Attack", true);//La animacion debe tener sincronizada con evento la accion de atacar, y luego la finalizacion del ataque.
										//Eso es muy desprolijo.
	}

	public void WantToAttack()
	{
		if (attackTimer <= 0)
		{
			_sm.SetState<EnemyRangedMetheoriteSpawner_AttackState>();
			attackTimer = attackCooldown;
		}
		else if(!_sm.IsActualState<EnemyRangedMetheoriteSpawner_CooldownState>())
			_sm.SetState<EnemyRangedMetheoriteSpawner_CooldownState>();
	}

	public override void RePosition()
	{
		if (_sm.IsActualState<EnemyRangedMetheoriteSpawner_PathfindingState>()) return;

		_sm.SetState<EnemyRangedMetheoriteSpawner_PathfindingState>();
	}

	public void WaitForCooldown()
	{
		_sm.SetState<EnemyRangedMetheoriteSpawner_CooldownState>();
	}

	public override bool CheckAttackConditions()
	{
		if (!new IsOnRange().Execute(transform.position, objective.transform.position, attackRange))
		{
			Debug.Log("No cumple con el rango");
			RePosition();
			return false;
		}

		if (!new IsOnSightView().Execute(transform.position, transform.forward, objective.transform.position, angleOfSight))
		{
			Debug.Log("No cumple con el angleofsight");
			RePosition();
			return false;
		}

		RaycastHit hit;
		if (Physics.Raycast(transform.position, (objective.transform.position-transform.position).normalized, out hit, obstaclesAndPlayerLayer))
		{
			Player player = hit.collider.GetComponent<Player>();
			if (player == null)
			{
				Debug.Log("No cumple con el Raycast");
				RePosition();
				return false;
			}
		}

		if (attackTimer > 0)
		{
			Debug.Log("No cumple con el Cooldown");
			WaitForCooldown();
			return false;
		}

		return true;
	}
}
public class EnemyRangedMetheoriteSpawner_SpawnState : State
{
	EnemyRangedMetheoriteSpawner owner;
	public EnemyRangedMetheoriteSpawner_SpawnState(StateMachineClassic sm, EnemyRangedMetheoriteSpawner owner) : base(sm)
	{
		this.owner = owner;
	}

	public override void Awake()
	{
		Debug.Log("Estado Spawn");
		base.Awake();
		owner.Spawn();
	}

	public override void Execute()
	{
		base.Execute();
		if (owner.IsSpawning()) return;
		if (owner.CheckAttackConditions()) owner.MakeAttack();
	}
}

public class EnemyRangedMetheoriteSpawner_AttackState : State
{
	EnemyRangedMetheoriteSpawner owner;
	public EnemyRangedMetheoriteSpawner_AttackState(StateMachineClassic sm, EnemyRangedMetheoriteSpawner owner) : base(sm)
	{
		this.owner = owner;
	}

	public override void Awake()
	{
		Debug.Log("Estado atacar");
		base.Awake();
		owner.StartAttackAnimation();
	}
}

public class EnemyRangedMetheoriteSpawner_CooldownState : State
{
	EnemyRangedMetheoriteSpawner owner;
	public EnemyRangedMetheoriteSpawner_CooldownState(StateMachineClassic sm, EnemyRangedMetheoriteSpawner owner) : base(sm)
	{
		this.owner = owner;
	}

	public override void Awake()
	{
		Debug.Log("Estado cooldown");
		base.Awake();
	}

	public override void Execute()
	{
		base.Execute();
		if (owner.CheckAttackConditions()) owner.MakeAttack();
	}
}

public class EnemyRangedMetheoriteSpawner_PathfindingState : State, IUsePathfinding
{
	EnemyRangedMetheoriteSpawner owner;
	List<Transform> path;
	List<ANode> nodes;
	Transform target;
	int index;

	public EnemyRangedMetheoriteSpawner_PathfindingState(StateMachineClassic sm, EnemyRangedMetheoriteSpawner owner, List<ANode> nodes) : base(sm)
	{
		this.owner = owner;
		path = new List<Transform>();
		this.nodes = nodes;
		target = owner.objective.transform;
		index = 0;
	}

	public override void Awake()
	{
		base.Awake();
		SetNewPath();
		index = 0;
	}

	public override void Execute()
	{
		base.Execute();
		if (owner.CheckAttackConditions()) owner.MakeAttack();
		FollowPath();
	}

	public override void Sleep()
	{
		base.Sleep();
		index = 0;
	}

	private void FollowPath()
	{
		if (path == null) return;
		if (IsEndPath())
		{
			index = 0;
			path = null;
			SetNewPath();
		}
		float range = 2f;
		Vector3 toPathIndex = new Vector3();
		if (path != null)
			toPathIndex = path[index].position - owner.transform.position;
		float distanceToNextWaypoint = toPathIndex.magnitude;
		if (distanceToNextWaypoint < range)
		{
			index++;
			return;
		}
		Vector3 dirInterpolation = Vector3.Lerp(owner.transform.forward, new Vector3(toPathIndex.x, 0, toPathIndex.z).normalized, owner.rotationSpeed * Time.deltaTime);
		owner.transform.forward = new Vector3(dirInterpolation.x, 0, dirInterpolation.z).normalized;
		owner.SetVelocity(owner.transform.forward);
		owner.animator.SetFloat("Speed", 1);
	}

	private void SetNewPath()
	{
		path = null;
		MyAStar.Instance.AddRequester(this);
	}

	public ANode GetFinalNode()
	{
		ANode candidateNode = null;
		float distanceDifference = Mathf.Infinity;
		foreach (ANode node in nodes)
		{
			float difference = GetDistanceDifference(node);
			if (difference < distanceDifference)
			{
				distanceDifference = difference;
				candidateNode = node;
			}
		}
		if (candidateNode == null)
		{
			Debug.LogError("ES EL FINAL NODE QUE ESTA DEVOLVIENDO NULO!");
		}
		return candidateNode;
	}

	private float GetDistanceDifference(ANode node)
	{
		float distanceToNode = (node.transform.position - target.position).magnitude;
		return Mathf.Abs(distanceToNode);
	}

	public ANode GetInitialNode()
	{
		ANode candidateNode = null;
		float nearestDistance = Mathf.Infinity;

		foreach (ANode node in nodes)
		{
			float distance = GetOwnerDistanceToNode(node);
			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				candidateNode = node;
			}
		}
		if (candidateNode == null)
		{
			Debug.LogError("ES EL #INITIAL# NODE QUE ESTA DEVOLVIENDO NULO!");
		}
		return candidateNode;
	}

	private float GetOwnerDistanceToNode(ANode node)
	{
		return Vector3.Distance(owner.transform.position, node.transform.position);
	}

	public void SetPath(Stack<ANode> path)
	{
		this.path = ConvertNodeListToTransformList(new List<ANode>(path.ToArray()));
	}

	private List<Transform> ConvertNodeListToTransformList(List<ANode> nodeList)
	{
		List<Transform> list = new List<Transform>();
		for (int i = nodeList.Count-1; i >= 0; i--)
		{
			list.Add(nodeList[i].transform);
		}
		return list;
	}

	private bool IsEndPath()
	{
		if (index >= path.Count)
		{
			return true;
		}
		return false;
	}
}


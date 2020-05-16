using UnityEngine;

public class EnemyRangedMetheoriteSpawner : EnemyRanged
{
	[Header("Enemy Metheorite Properties")]
	[SerializeField] string enemyName;
	[SerializeField] Animator animator;
	StateMachineClassic _sm;
	float attackTimer;

	private void Start()
	{
		_sm = new StateMachineClassic();
		_sm.AddState(new EnemyRangedMetheoriteSpawner_AttackState(_sm, this));
		_sm.AddState(new EnemyRangedMetheoriteSpawner_SpawnState(_sm, this));
		_sm.AddState(new EnemyRangedMetheoriteSpawner_CooldownState(_sm, this));
		attackTimer = 0;
		_sm.SetState<EnemyRangedMetheoriteSpawner_SpawnState>();
	}

	private void Update()
	{
		attackTimer -= Time.deltaTime;
		_sm.Update();
	}

	public override void Attack()
	{
		base.Attack();
		Debug.Log("Ataca!");
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
		Debug.Log("Intentando re posicionarse");
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
		if (!owner.IsSpawning())
			owner.WantToAttack();
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
		if (owner.IsObjectiveOnRange())
		{
			Debug.Log("Aloha hello there");
			owner.WantToAttack();
			return;
		}
		owner.RePosition();
	}
}


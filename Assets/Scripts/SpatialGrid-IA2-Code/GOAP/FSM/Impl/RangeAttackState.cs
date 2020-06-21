using System;
using FSM;
using UnityEngine;

public class RangeAttackState : MonoBaseState {

	public float attackRate;
	public float manaCost;

	private GOAPEnemy owner;
	[SerializeField] TripleBombAttack attackPrefab;
	private float _lastAttackTime;

	private void Awake()
	{
		owner = GetComponent<GOAPEnemy>();
	}

	public override void UpdateLoop() {
		if (Time.time >= _lastAttackTime + attackRate && owner.mana >= manaCost)
		{
			_lastAttackTime = Time.time;
			var attack = Instantiate(attackPrefab);
			attack.transform.position = this.transform.position;
			owner.mana -= manaCost;
		}

		EventsManager.TriggerEvent("RePlan", owner);
    }

    public override IState ProcessInput() {
		return this;
    }
}
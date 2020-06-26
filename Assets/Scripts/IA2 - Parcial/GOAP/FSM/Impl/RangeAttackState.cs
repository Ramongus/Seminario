using System;
using FSM;
using UnityEngine;
//IA2-P2
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
            owner.manaImage.fillAmount = owner.mana / 50;
            var t = transform.Find("ENEMYMELE");
            t.GetComponent<Animator>().SetTrigger("AreaAttack");
        }
	}

    public override IState ProcessInput() {
		return this;
    }
}
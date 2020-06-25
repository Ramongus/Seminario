using System;
using FSM;
using UnityEngine;

public class ChargeManaState : MonoBaseState {

	GOAPEnemy owner;

	[SerializeField] float manaChargeScale;


	private void Awake()
	{
		owner = GetComponent<GOAPEnemy>();
	}

	public override void UpdateLoop() {
		owner.mana += Time.deltaTime * manaChargeScale;
		owner.mana = Mathf.Clamp(owner.mana, 0, owner.maxMana);
		Debug.Log("Cargando el/la mana....");
    }

    public override IState ProcessInput() {

		/*if (owner.mana >= owner.maxMana)
		{
			owner.mana = owner.maxMana;
			return Transitions["OnPatrolState"];
			//EventsManager.TriggerEvent("RePlan", owner);
		}*/

		return this;
    }
}
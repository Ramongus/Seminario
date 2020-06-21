using System;
using FSM;
using UnityEngine;

public class ChargeManaState : MonoBaseState {

	GOAPEnemy owner;//MANA

	[SerializeField] float manaChargeScale;


	private void Awake()
	{
		owner = GetComponent<GOAPEnemy>();
	}

	public override void UpdateLoop() {
		owner.mana += Time.deltaTime * manaChargeScale;
		Debug.Log("Cargando el/la mana....");
    }

    public override IState ProcessInput() {

		if (owner.mana >= owner.maxMana)
		{
			owner.mana = owner.maxMana;
			return Transitions["OnPatrolState"];
			//EventsManager.TriggerEvent("RePlan", owner);
		}

		return this;
    }
}
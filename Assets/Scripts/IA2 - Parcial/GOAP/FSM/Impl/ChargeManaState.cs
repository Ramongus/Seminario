using System;
using FSM;
using UnityEngine;
//IA2-P2
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
        var t = transform.Find("ENEMYMELE");
        owner.manaImage.fillAmount = owner.mana / 50;
        t.GetComponent<Animator>().SetFloat("Speed",0);
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
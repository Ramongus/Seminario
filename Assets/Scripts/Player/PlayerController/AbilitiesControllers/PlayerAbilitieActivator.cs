using UnityEngine;

public class PlayerAbilitieActivator
{
	Transform aimPointer;

	public PlayerAbilitieActivator(Transform aimPointer)
	{
		this.aimPointer = aimPointer;
	}

	public void ActivateHabilitie()
	{
		/*
		if (Input.GetButtonDown("Fire"))
		{
			EventsManager.TriggerEvent("FireHabilitie", aimPointer.position);
		}
		*/

		if (Input.GetKeyUp(KeyCode.Q))
		{
			EventsManager.TriggerEvent("SelectAbilitie", 0);
			EventsManager.TriggerEvent("FireHabilitie", aimPointer.position);
		}

		if (Input.GetKeyUp(KeyCode.E))
		{
			EventsManager.TriggerEvent("SelectAbilitie", 1);
			EventsManager.TriggerEvent("FireHabilitie", aimPointer.position);
		}

		if (Input.GetKeyUp(KeyCode.R))
		{
			EventsManager.TriggerEvent("CreateQuery", 0, aimPointer.position);
		}
	}
}

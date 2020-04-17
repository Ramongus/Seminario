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
		if (Input.GetButtonDown("Fire"))
		{
			EventsManager.TriggerEvent("FireHabilitie", aimPointer.position);
		}
	}
}

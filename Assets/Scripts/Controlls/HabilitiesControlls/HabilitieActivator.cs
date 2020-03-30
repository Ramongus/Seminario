using UnityEngine;

public class HabilitieActivator
{
	Transform aimPointer;

	public HabilitieActivator(Transform aimPointer)
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

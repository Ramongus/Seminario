using UnityEngine;

public class HabilitiesSelectorController
{
	public void UpdateChangeHabilitie()
	{
		if (Input.GetButtonDown("NextHabilitie"))
		{
			EventsManager.TriggerEvent("NextHabilitie");
		}

		if (Input.GetButtonDown("PreviousHabilitie"))
		{
			EventsManager.TriggerEvent("PreviousHabilitie");
		}
	}
}

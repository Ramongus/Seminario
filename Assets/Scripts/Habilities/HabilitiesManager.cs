using System;
using System.Collections.Generic;
using UnityEngine;

public class HabilitiesManager
{
	List<AbstractHabilities> myHabilities;
	int habilitieIndex;

	Player owner;

	public HabilitiesManager(List<AbstractHabilities> theHabilities, Player player)
	{
		owner = player;
		myHabilities = new List<AbstractHabilities>(theHabilities);
		habilitieIndex = 0;
		EventsManager.SuscribeToEvent("NextHabilitie", ChangeToNextHabilitie);
		EventsManager.SuscribeToEvent("PreviousHabilitie", ChangeToPreviousHabilitie);
		EventsManager.SuscribeToEvent("FireHabilitie", CastHabilitie);
	}

	public void CastHabilitie(params object[] parameters)
	{
		EventsManager.TriggerEvent("CastHabilitie", myHabilities[habilitieIndex].GetName(), parameters[0], owner.transform.position);
	}

	public void ChangeToNextHabilitie(params object[] parameters)
	{
		habilitieIndex++;
		if (habilitieIndex >= myHabilities.Count)
			habilitieIndex = 0;
	}

	public void ChangeToPreviousHabilitie(params object[] parameters)
	{
		habilitieIndex--;
		if (habilitieIndex < 0)
			habilitieIndex = myHabilities.Count - 1;
	}
}

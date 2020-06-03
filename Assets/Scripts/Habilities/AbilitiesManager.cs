using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesManager
{
	List<AbstractAbilities> myHabilities;
	int habilitieIndex;

	ICastAbilities _owner;

	public AbilitiesManager(List<AbstractAbilities> theHabilities, ICastAbilities owner)
	{
		_owner = owner;
		myHabilities = new List<AbstractAbilities>(theHabilities);
		habilitieIndex = 2;
		EventsManager.SuscribeToEvent("NextHabilitie", ChangeToNextHabilitie);
		EventsManager.SuscribeToEvent("PreviousHabilitie", ChangeToPreviousHabilitie);
		EventsManager.SuscribeToEvent("SelectAbilitie", ChangeToAbilitie);
		EventsManager.SuscribeToEvent("FireHabilitie", CastHabilitie);
	}

	private void ChangeToAbilitie(object[] parameters)
	{
		var index = (int)parameters[0];
		habilitieIndex = index;
	}

	public void CastHabilitie(params object[] parameters)
	{
		EventsManager.TriggerEvent("CastHabilitie", myHabilities[habilitieIndex].GetName(), parameters[0], _owner.GetPosition());
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

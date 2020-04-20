using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesInstantiator : MonoBehaviour
{
	[SerializeField]
	public List<AbstractAbilities> habilitiesPrefabs;

	private void Awake()
	{
		EventsManager.SuscribeToEvent("CastHabilitie", CastHabilitie);
	}

	public void CastHabilitie(params object[] parameters)
	{
		string habilitieName = (string)parameters[0];
		foreach (AbstractAbilities habilitie in habilitiesPrefabs)
		{
			if (habilitie.IsHabilitieName(habilitieName))
				Instantiate(habilitie).SetInitiation((Vector3)parameters[1], (Vector3)parameters[2]);
		}
	}

	public AbstractAbilities BuildHabilitie(string habilitieName)
	{
		foreach (AbstractAbilities habilitie in habilitiesPrefabs)
		{
			if (habilitie.IsHabilitieName(habilitieName))
				return Instantiate(habilitie);
		}
		return null;
	}


}

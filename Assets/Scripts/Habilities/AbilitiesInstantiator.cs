using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesInstantiator : MonoBehaviour
{
	[SerializeField]
	public List<AbstractAbilities> habilitiesPrefabs;

	[SerializeField]
	public List<float> habilitiesCooldowns;
	List<float> cooldownsTimer;

	private void Awake()
	{
		EventsManager.SuscribeToEvent("CastHabilitie", CastHabilitie);
		cooldownsTimer = new List<float>(habilitiesCooldowns);
		for (int i = 0; i < cooldownsTimer.Count; i++)
		{
			cooldownsTimer[i] = 0;
		}
	}

	private void Update()
	{
		for (int i = 0; i < habilitiesCooldowns.Count; i++)
		{
			cooldownsTimer[i] -= Time.deltaTime;
		}
	}

	public void CastHabilitie(params object[] parameters)
	{
		string habilitieName = (string)parameters[0];
		/*
		foreach (AbstractAbilities habilitie in habilitiesPrefabs)
		{
			if (habilitie.IsHabilitieName(habilitieName))
				Instantiate(habilitie).SetInitiation((Vector3)parameters[1], (Vector3)parameters[2]);
		}
		*/
		for (int i = 0; i < habilitiesPrefabs.Count; i++)
		{
			if (habilitiesPrefabs[i].IsHabilitieName(habilitieName))
				if(cooldownsTimer[i] <= 0)
				{
					Instantiate(habilitiesPrefabs[i]).SetInitiation((Vector3)parameters[1], (Vector3)parameters[2]);
					cooldownsTimer[i] = habilitiesCooldowns[i];
					EventsManager.TriggerEvent("AbilitieCasted");
				}
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

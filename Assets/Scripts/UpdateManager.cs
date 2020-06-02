using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
	List<IUpdate> wantsToUpdate;
	List<IUpdate> updating;
	List<IUpdate> wantsToStopUpdating;
	bool update;

	private void Awake()
	{
		update = true;
		wantsToUpdate = new List<IUpdate>();
		updating = new List<IUpdate>();
		wantsToStopUpdating = new List<IUpdate>();
		EventsManager.SuscribeToEvent("SuscribeToUpdateManager", AddToUpdateCicle);
		EventsManager.SuscribeToEvent("UnsuscribeToUpdateManager", RemoveFromUpdateCicle);
		EventsManager.SuscribeToEvent("OnPlayerDie", NoMoreUpdate);
		EventsManager.SuscribeToEvent("PlayerResurrect", UpdateAgain);
	}


	void Update()
    {
		if (update)
		{	
			foreach (IUpdate item in updating)
			{
				item.MyUpdate();
			}
		}
    }

	private void UpdateAgain(object[] parameters)
	{
		update = true;
	}

	public void NoMoreUpdate(params object[] parameters)
	{
		update = false;
	}

	private void LateUpdate()
	{
		foreach (IUpdate item in wantsToUpdate)
		{
			updating.Add(item);
		}
		foreach (IUpdate item in wantsToStopUpdating)
		{
			updating.Remove(item);
		}

		wantsToUpdate.Clear();
		wantsToStopUpdating.Clear();
	}

	public void AddToUpdateCicle(params object[] parameters)
	{
		IUpdate obj = (IUpdate)parameters[0];
		if(!updating.Contains(obj))
			wantsToUpdate.Add(obj);
	}

	public void RemoveFromUpdateCicle(params object[] parameters)
	{
		IUpdate obj = (IUpdate)parameters[0];
		if (updating.Contains(obj))
			wantsToStopUpdating.Add(obj);
	}
}

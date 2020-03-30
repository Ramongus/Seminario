using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventsManager
{
	public delegate void EventCallback(params object[] parameters);
	public static Dictionary<string, EventCallback> events;

	public static void SuscribeToEvent(string eventName, EventCallback callback)
	{
		if(events == null)
		{
			events = new Dictionary<string, EventCallback>();
		}

		if (events.ContainsKey(eventName))
		{
			events[eventName] += callback;
		}
		else
		{
			events.Add(eventName, callback);
		}
	}

	public static void UnSuscribeToEvent(string eventName, EventCallback callback)
	{
		if (events.ContainsKey(eventName))
		{
			events[eventName] -= callback;
		}
	}

	public static void TriggerEvent(string eventName, params object[] parameters)
	{
		if(events != null)
		{
			if (events.ContainsKey(eventName))
			{
				events[eventName](parameters);
				return;
			}
		}
		Debug.LogWarning("No existe el evento que se esta intentado disparar! Checkee que el nombre del evento sea el correcto o que haya al menos 1 suscriptor.");
	}

	public static void DeleteAllSuscribedEvents()
	{
		if(events != null)
			events.Clear();
	}
}

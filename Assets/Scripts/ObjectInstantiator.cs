using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstantiator : MonoBehaviour
{

	private void Awake()
	{
		EventsManager.SuscribeToEvent("Instantiate", InstantiateObject);
	}

	public void InstantiateObject(params object[] parameters)
	{

	}
}

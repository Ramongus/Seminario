using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueryCreator : MonoBehaviour
{
	//IA2-P1
	public List<GameObject> querys;

	private void Awake()
	{
		EventsManager.SuscribeToEvent("CreateQuery", CreateQuery);
	}

	//param1:QueryIndex (int)
	//param2: QuerySpawnPoint (Vector3)
	private void CreateQuery(object[] parameters)
	{
		int index = (int)parameters[0];
		Vector3 pos = (Vector3)parameters[1];

		var q = Instantiate(querys[index]);
		q.transform.position = pos;
	}
}

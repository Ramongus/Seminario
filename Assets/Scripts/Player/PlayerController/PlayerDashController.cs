using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashController
{
	Transform owner;
	float timeDashing;
	float dashDistance;

	//Transform owner, float timeDashing, float dashDistance

	public PlayerDashController()
	{

	}

	public void CheckDash()
	{
		if(Input.GetKeyDown(KeyCode.Space))
			EventsManager.TriggerEvent("Dash");
	}
}

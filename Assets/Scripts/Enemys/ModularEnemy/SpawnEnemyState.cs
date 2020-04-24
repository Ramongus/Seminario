using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyState : MonoBehaviour, IState
{
	[SerializeField] string stateName;
	[SerializeField] float spawnTime;
	[SerializeField] Material defaultMaterial;
	[SerializeField] Material spawnMaterial;
	[SerializeField] float spawnDissapearValue;
	[SerializeField] float spawnAppearValue;
	[SerializeField] GameObject render;
	[SerializeField] string nextStateName;
	float timer;

	StateMachine myStateMachine;

	private void Start()
	{
		SetStateMachine();
		timer = spawnTime;
		spawnMaterial.SetFloat("_Teleport", spawnDissapearValue);
		SetMaterial(spawnMaterial);
	}

	public string GetStateName()
	{
		return stateName;
	}

	public void SetStateMachine()
	{
		myStateMachine = GetComponent<StateMachine>();
	}

	public void StateAwake()
	{
		SetMaterial(spawnMaterial);
	}

	public void StateExecute()
	{
		Debug.Log("ON SPAWN STATE");
		timer -= Time.deltaTime;
		float spawnCompleted = (spawnTime - timer) / spawnTime;
		float spawnMaterialFillAmount = Mathf.Lerp(spawnDissapearValue, spawnAppearValue, spawnCompleted);
		Debug.Log("_Teleport value: " + spawnMaterialFillAmount);
		spawnMaterial.SetFloat("_Teleport", spawnMaterialFillAmount);
		if(timer <= 0)
		{
			myStateMachine.SetStateByName(nextStateName);
			return;
		}
	}

	public void StateSleep()
	{
		SetMaterial(defaultMaterial);
	}

	public void SetMaterial(Material currentMaterial)
	{
		render.GetComponent<Renderer>().material = currentMaterial;
	}
}

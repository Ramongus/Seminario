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

	[SerializeField] Collider[] collidersToActive;
	float timer;

	Material spawnMaterialInstance;

	StateMachine myStateMachine;

	private void Start()
	{
		spawnMaterialInstance = new Material(spawnMaterial);
		SetStateMachine();
		timer = spawnTime;
		spawnMaterialInstance.SetFloat("_Teleport", spawnDissapearValue);
		SetMaterial(spawnMaterialInstance);
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
		SetMaterial(spawnMaterialInstance);
	}

	public void StateExecute()
	{
		Debug.Log("ON SPAWN STATE");
		timer -= Time.deltaTime;
		float spawnCompleted = (spawnTime - timer) / spawnTime;
		float spawnMaterialFillAmount = Mathf.Lerp(spawnDissapearValue, spawnAppearValue, spawnCompleted);
		spawnMaterialInstance.SetFloat("_Teleport", spawnMaterialFillAmount);
		if(timer <= 0)
		{
			myStateMachine.SetStateByName(nextStateName);
			return;
		}
	}

	public void StateSleep()
	{
		SetMaterial(defaultMaterial);
		for (int i = 0; i < collidersToActive.Length; i++)
		{
			collidersToActive[i].enabled = true;
		}
		if(GetComponent<Rigidbody>() != null)
			GetComponent<Rigidbody>().isKinematic = false;
	}

	public void SetMaterial(Material currentMaterial)
	{
		render.GetComponent<Renderer>().material = currentMaterial;
	}
}

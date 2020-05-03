using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyState : MonoBehaviour, IState
{
	[SerializeField] protected string stateName;
	[SerializeField] protected float spawnTime;
	[SerializeField] protected Material defaultMaterial;
	[SerializeField] protected Material spawnMaterial;
	[SerializeField] protected float spawnDissapearValue;
	[SerializeField] protected float spawnAppearValue;
	[SerializeField] protected GameObject render;
	[SerializeField] protected string nextStateName;
	[SerializeField] protected GameObject enemyCanvas;

	[SerializeField] protected Collider[] collidersToActive;
	protected float timer;

	protected Material spawnMaterialInstance;

	protected StateMachine myStateMachine;

	protected virtual void Start()
	{
		SetStateMachine();
		timer = spawnTime;
		spawnMaterialInstance = new Material(spawnMaterial);
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

	public virtual void StateAwake()
	{
		Debug.Log("ON SPAWN STATE");
		spawnMaterialInstance.SetFloat("_Teleport", spawnDissapearValue);
		SetMaterial(spawnMaterialInstance);
		timer = spawnTime;
	}

	public virtual void StateExecute()
	{
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

	public virtual void StateSleep()
	{
		if (enemyCanvas != null)
			enemyCanvas.SetActive(true);
		SetMaterial(defaultMaterial);
		for (int i = 0; i < collidersToActive.Length; i++)
		{
			collidersToActive[i].enabled = true;
		}
		if(GetComponent<Rigidbody>() != null)
		{
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}

	}

	public void SetMaterial(Material currentMaterial)
	{
		render.GetComponent<Renderer>().material = currentMaterial;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearState : MonoBehaviour, IStateMono
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
		float aux = spawnAppearValue;
		spawnAppearValue = spawnDissapearValue;
		spawnDissapearValue = aux;

		SetStateMachine();
		timer = spawnTime;
		spawnMaterialInstance = new Material(spawnMaterial);
		spawnMaterialInstance.SetFloat("_Teleport", spawnDissapearValue);
	}


	public virtual void StateAwake()
	{
		Debug.Log("ON Dissapear STATE");
		if(enemyCanvas != null)
			enemyCanvas.SetActive(false);
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
		if (timer <= 0)
		{
			myStateMachine.SetStateByName(nextStateName);
			return;
		}
	}

	public virtual void StateSleep()
	{
	
	}

	public void SetMaterial(Material currentMaterial)
	{
		render.GetComponent<Renderer>().material = currentMaterial;
	}

	public string GetStateName()
	{
		return stateName;
	}

	public void SetStateMachine()
	{
		myStateMachine = GetComponent<StateMachine>();
	}
}

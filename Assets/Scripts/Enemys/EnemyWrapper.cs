using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWrapper : MonoBehaviour
{
	[Header("Spawn Properties")]
	[SerializeField] protected float spawnTime;
	[SerializeField] protected Material defaultMaterial;
	[SerializeField] protected Material spawnMaterial;
	[SerializeField] protected float spawnDissapearValue;
	[SerializeField] protected float spawnAppearValue;
	[SerializeField] protected GameObject render;
	[SerializeField] protected GameObject enemyCanvas;
	[SerializeField] protected Collider[] collidersToActive;

	protected float timer;
	protected Material spawnMaterialInstance;
	protected bool isSpawning;

	private void Awake()
	{
		Spawn();
	}

	public virtual void Spawn()
	{
		isSpawning = true;
		timer = spawnTime;
		spawnMaterialInstance = new Material(spawnMaterial);
		spawnMaterialInstance.SetFloat("_Teleport", spawnDissapearValue);
		SetMaterial(spawnMaterialInstance);
		StartCoroutine(SpawnCicle());
	}

	protected IEnumerator SpawnCicle()
	{
		while (isSpawning)
		{
			timer -= Time.deltaTime;
			float spawnCompleted = (spawnTime - timer) / spawnTime;
			float spawnMaterialFillAmount = Mathf.Lerp(spawnDissapearValue, spawnAppearValue, spawnCompleted);
			spawnMaterialInstance.SetFloat("_Teleport", spawnMaterialFillAmount);
			if (timer <= 0)
			{
				isSpawning = false;
				FinishSpawn();
			}
			yield return null;
		}
	}

	protected void FinishSpawn()
	{
		if (enemyCanvas != null)
			enemyCanvas.SetActive(true);
		SetMaterial(defaultMaterial);
		for (int i = 0; i < collidersToActive.Length; i++)
		{
			collidersToActive[i].enabled = true;
		}
		if (GetComponent<Rigidbody>() != null)
		{
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	protected void SetMaterial(Material currentMaterial)
	{
		render.GetComponent<Renderer>().material = currentMaterial;
	}
}
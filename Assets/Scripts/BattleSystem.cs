using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
	[SerializeField] Wave[] waves;
	[SerializeField] ColliderTrigger colliderTrigger;
	bool isActivated;

	private void Start()
	{
		colliderTrigger.OnPlayerEnterTrigger += ColliderTrigger_OnPlayerEnterTrigger;
		for (int i = 0; i < waves.Length; i++)
		{
			EnemySpawn[] enemiesInWave = waves[i].GetEnemies();
			for (int j = 0; j < enemiesInWave.Length; j++)
			{
				enemiesInWave[j].OnEnemyDies += BattleSystem_OnEnemyDies;
			}
		}
	}

	private void BattleSystem_OnEnemyDies(object sender, EventArgs e)
	{
		if(AreWavesOver())
			StopBattle();
	}

	private void ColliderTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e)
	{
		StartBattle();
		colliderTrigger.OnPlayerEnterTrigger -= ColliderTrigger_OnPlayerEnterTrigger;
	}

	private void Update()
	{
		if (isActivated)
		{	
			for (int i = 0; i < waves.Length; i++)
			{
				waves[i].Update();
			}
		}
	}

	private void StartBattle()
	{
		Debug.Log("StartBattle");
		isActivated = true;
	}

	private void StopBattle()
	{
		EventsManager.TriggerEvent("SoulCompleted");
	}

	public bool AreWavesOver()
	{
		for (int i = 0; i < waves.Length; i++)
		{
			if (!waves[i].WaveIsOver())
				return false;
		}
		return true;
	}

	[System.Serializable]
	private class Wave
	{
		[SerializeField] EnemySpawn[] enemysToSpawn;
		[SerializeField] float timer;

		public void Update()
		{
			if(timer > 0)
			{
				timer -= Time.deltaTime;
			}
			if(timer <= 0)
			{
				SpawnEnemies();
			}
		}
		
		public void SpawnEnemies()
		{
			for (int i = 0; i < enemysToSpawn.Length; i++)
			{
				enemysToSpawn[i].SpawnEnemy();
			}
		}

		public bool WaveIsOver()
		{
			for (int i = 0; i < enemysToSpawn.Length; i++)
			{
				if (enemysToSpawn[i].IsAlive())
				{
					return false;
				}
			}
			return true;
		}

		public EnemySpawn[] GetEnemies()
		{
			return enemysToSpawn;
		}
	}
}


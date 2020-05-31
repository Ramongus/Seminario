using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRestarter : MonoBehaviour
{
	[SerializeField] float timeToRestartAfterFalling;
	[SerializeField] Transform playerSpawnPositionAndRotation;

	private void Awake()
	{
		EventsManager.SuscribeToEvent("RestartLevel", RestartLevel);
	}

	private void RestartLevel(object[] parameters)
	{
		StartCoroutine(CountDownToRestartLevel());
	}

	IEnumerator CountDownToRestartLevel()
	{
		yield return new WaitForSeconds(timeToRestartAfterFalling);
		RePositionPlayer();
	}

	private void RePositionPlayer()
	{
		Player player = FindObjectOfType<Player>();
		player.transform.position = playerSpawnPositionAndRotation.position;
		player.transform.rotation = playerSpawnPositionAndRotation.rotation;
		player.SetInitialValues();
	}
}

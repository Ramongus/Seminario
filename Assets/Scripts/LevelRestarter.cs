using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestarter : MonoBehaviour
{
	[SerializeField] float timeToRestartAfterFalling;
	[SerializeField] Transform playerSpawnPositionAndRotation;

	private void Awake()
	{
		EventsManager.SuscribeToEvent("OnPlayerDie", RestartLevel);
		EventsManager.SuscribeToEvent("RestartLevel", RestartLevel);
		EventsManager.SuscribeToEvent("StopAllCoroutines", StopCoroutines);
	}

	private void StopCoroutines(object[] parameters)
	{
		StopAllCoroutines();
	}

	private void RestartLevel(object[] parameters)
	{
		Debug.Log("Se llama");
		StartCoroutine(CountDownToRestartLevel());
	}

	IEnumerator CountDownToRestartLevel()
	{

		yield return new WaitForSeconds(timeToRestartAfterFalling);
		EventsManager.DeleteAllSuscribedEvents();
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		StopAllCoroutines();
		//RePositionPlayer();
	}

	/*
	private void RePositionPlayer()
	{
		Player player = FindObjectOfType<Player>();
		player.transform.position = playerSpawnPositionAndRotation.position;
		player.transform.rotation = playerSpawnPositionAndRotation.rotation;
		player.SetInitialValues();
		EventsManager.TriggerEvent("PlayerResurrect");
	}
	*/
}

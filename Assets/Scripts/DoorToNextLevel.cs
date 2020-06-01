using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToNextLevel : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Player player = other.GetComponent<Player>();
		if(player != null)
		{
			EventsManager.TriggerEvent("StopAllCoroutines");
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}
}

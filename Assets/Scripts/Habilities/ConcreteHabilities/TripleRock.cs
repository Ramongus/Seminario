using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleRock : AbstractAbilities
{
	[SerializeField] RockProyectile rockPrefab;
	[SerializeField] int rockQuantity;
	[SerializeField] float scaleMultiplier;
	//[SerializeField] float initialHeight;
	[SerializeField] float timeBetweenRocks;
	//Se instancian 3 rocas encima del player
	//Lanza de a una cada cierto tiempo
	float timer;

	public override void SetInitiation(Vector3 castPos, Vector3 playerPos)
	{
		StartCoroutine(SpawnRocks());
	}

	IEnumerator SpawnRocks()
	{
		Player player = FindObjectOfType<Player>();
		EventsManager.TriggerEvent("StartCastingSpell");
		EventsManager.TriggerEvent("CastTripleAttack");
		for (int i = 0; i < rockQuantity; i++)
		{
			var rock = Instantiate(rockPrefab);
			rock.SetInitiation(player.transform.position + player.transform.forward*5, player.transform.position);
			yield return new WaitForSeconds(timeBetweenRocks);
		}
		EventsManager.TriggerEvent("StopCastingSpell");
		yield return null;
	}
}

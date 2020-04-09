using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public float lifetime;
	public float explodeRadius;
	public GameObject explodeParticles;
	public float explosionDamage;

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(AutoDestruct());
    }

	private void OnCollisionEnter(Collision collision)
	{
		Player player = collision.gameObject.GetComponent<Player>();
		if(collision.gameObject.layer == LayerMask.NameToLayer("Arena") || player != null)
			Explode();
	}

	private void OnTriggerEnter(Collider other)
	{
		Player player = other.gameObject.GetComponent<Player>();
		if (other.gameObject.layer == LayerMask.NameToLayer("Arena") || player != null)
			Explode();
	}

	private void Explode()
	{
		Collider[] inExplodeRange = GetCollidersInExplodeRange();
		List<Player> playerInExplode = GetPlayersInExplosion(inExplodeRange);
		DoDamageToPlayers(playerInExplode);
		ExplodeAnimation();
		Destroy(gameObject);
	}

	private void ExplodeAnimation()
	{
		GameObject particles = Instantiate(explodeParticles);
		particles.transform.position = this.transform.position;
	}

	private Collider[] GetCollidersInExplodeRange()
	{
		return Physics.OverlapSphere(transform.position, explodeRadius);
	}

	private void DoDamageToPlayers(List<Player> playerInExplode)
	{
		foreach (Player player in playerInExplode)
		{
			player.SetHealth(player.GetHP() - explosionDamage);
		}
	}

	private List<Player> GetPlayersInExplosion(Collider[] inExplodeRange)
	{
		List<Player> players = new List<Player>();
		foreach (Collider collider in inExplodeRange)
		{
			Player isPlayer = collider.gameObject.GetComponent<Player>();
			if (isPlayer)
				players.Add(isPlayer);
		}
		return players;
	}

	private IEnumerator AutoDestruct()
	{
		yield return new WaitForSeconds(lifetime);
		Destroy(gameObject);
	}
}

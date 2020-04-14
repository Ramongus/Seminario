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
	public GameObject explodeField;

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(AutoDestruct());
    }

	public void SetExplodeField(GameObject explodeField)
	{
		this.explodeField = explodeField;
	}

	private void OnCollisionEnter(Collision collision)
	{
		PlayerView player = collision.gameObject.GetComponent<PlayerView>();
		if(collision.gameObject.layer == LayerMask.NameToLayer("Arena") || player != null)
			Explode();
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayerView player = other.gameObject.GetComponent<PlayerView>();
		if (other.gameObject.layer == LayerMask.NameToLayer("Arena") || player != null)
			Explode();
	}

	private void Explode()
	{
		Collider[] inExplodeRange = GetCollidersInExplodeRange();
		List<PlayerView> playerInExplode = GetPlayersInExplosion(inExplodeRange);
		DoDamageToPlayers(playerInExplode);
		ExplodeAnimation();
		if (explodeField != null)
			Destroy(explodeField);
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

	private void DoDamageToPlayers(List<PlayerView> playerInExplode)
	{
		foreach (PlayerView player in playerInExplode)
		{
			player.SetHealth(player.GetHP() - explosionDamage);
		}
	}

	private List<PlayerView> GetPlayersInExplosion(Collider[] inExplodeRange)
	{
		List<PlayerView> players = new List<PlayerView>();
		foreach (Collider collider in inExplodeRange)
		{
			PlayerView isPlayer = collider.gameObject.GetComponent<PlayerView>();
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

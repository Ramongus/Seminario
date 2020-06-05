using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedPortals : MonoBehaviour
{
	[SerializeField] ConnectedPortals portalConnected;

	private void OnTriggerEnter(Collider other)
	{
		Player player = other.GetComponent<Player>();
		if(player != null)
		{
			portalConnected.GetComponent<Collider>().enabled = false;
			player.transform.position = new Vector3(portalConnected.transform.position.x, player.transform.position.y, portalConnected.transform.position.z);
		}
	}
}

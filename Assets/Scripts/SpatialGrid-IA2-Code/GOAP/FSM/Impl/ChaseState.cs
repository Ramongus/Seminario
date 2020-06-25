using System.Collections.Generic;
using FSM;
using UnityEngine;

public class ChaseState : MonoBaseState {

	private GOAPEnemy owner;
    public float speed = 2f;
    
    public float rangeDistance = 5;
    public float meleeDistance = 1.5f;
    
    private Player _player;

	private Vector3 dir;
	[SerializeField] private float sightRange;
	[SerializeField] private LayerMask obstacleLayer;

	private void Awake() {
        _player = FindObjectOfType<Player>();
		owner = GetComponent<GOAPEnemy>();
    }

    public override void UpdateLoop() {

		Debug.LogWarning("On ChaseState");

        dir = (_player.transform.position - transform.position).normalized;

		dir = new Vector3(dir.x, 0, dir.z).normalized;

        transform.position += dir * (speed * Time.deltaTime);
    }

	public override IState ProcessInput() {
		/*var playerPosAtMyHeight = new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
		var sqrDistance = (playerPosAtMyHeight - transform.position).sqrMagnitude;
		var dist = (playerPosAtMyHeight - transform.position).magnitude;

		bool isOnSight = true;
		RaycastHit hit;
		Physics.Raycast(transform.position, dir, out hit, dist, obstacleLayer);
		if (hit.collider != null)
		{
			isOnSight = false;
		}

		if(!isOnSight)
		{
			Debug.LogWarning("!!! RE PLANEA !!!");
			EventsManager.TriggerEvent("RePlan", owner);
			return this;
		}

		if (sqrDistance < rangeDistance * rangeDistance && Transitions.ContainsKey("OnRangeAttackState"))
		{
			return Transitions["OnRangeAttackState"];
		}

		if (sqrDistance < meleeDistance * meleeDistance && Transitions.ContainsKey("OnMeleeAttackState")) {
            return Transitions["OnMeleeAttackState"];
        }*/

        return this;
    }
}
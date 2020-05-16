using UnityEngine;

public class RangedAttackMetheorite : EnemyRangedAttack
{
	[SerializeField] float initialHeight;
	[SerializeField] int floorLayerNumber;
	[SerializeField] float gravityBust;

	Rigidbody rigi;

	private void Start()
	{
		rigi = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		rigi.velocity = rigi.velocity + Vector3.up * gravityBust * Time.deltaTime;
	}

	public override void PositionAttack(GameObject owner, GameObject objective)
	{
		transform.position = objective.transform.position + Vector3.up * initialHeight;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.layer == floorLayerNumber)
		{
			Destroy(this.gameObject);
		}

		Player player = collision.gameObject.GetComponent<Player>();
		if (player == null) return;
		player.SetHealth(player.GetHealth() - damage);
	}
}

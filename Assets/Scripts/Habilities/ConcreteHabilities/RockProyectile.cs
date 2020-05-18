using UnityEngine;

public class RockProyectile : AbstractAbilities
{
	[SerializeField] float speed;

	protected override void Update()
	{
		base.Update();
		transform.position += transform.forward * speed * Time.deltaTime;
	}

	public override void SetInitiation(Vector3 castPos, Vector3 playerPos)
	{
		var dir = (castPos - playerPos).normalized;
		transform.forward = dir;
		transform.position = playerPos + dir * initialHeight + Vector3.up*initialHeight;

	}
}

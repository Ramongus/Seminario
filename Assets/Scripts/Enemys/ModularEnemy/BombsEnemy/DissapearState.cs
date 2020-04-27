using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearState : SpawnEnemyState
{
	protected override void Start()
	{
		float aux = spawnAppearValue;
		spawnAppearValue = spawnDissapearValue;
		spawnDissapearValue = aux;
		base.Start();
	}

	public override void StateSleep()
	{
		base.StateSleep();
		spawnMaterialInstance.SetFloat("_Teleport", spawnAppearValue);
		SetMaterial(spawnMaterialInstance);
	}
}

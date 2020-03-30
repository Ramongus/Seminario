using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
	[SerializeField] List<AbstractHabilities> myHabilities;


	[SerializeField] float aimSensitivity;
	[SerializeField] Transform aimPointer;

	[SerializeField] float movementSpeed;

	PlayerLogic logic;

	HabilitiesManager myHabilitiesManager;


	private void Awake()
	{
		logic = new PlayerLogic(transform, movementSpeed, aimPointer, aimSensitivity);
		myHabilitiesManager = new HabilitiesManager(myHabilities, this);
	}

	private void Update()
	{
		logic.Logic();
	}

	public float GetDamage()
	{
		throw new System.NotImplementedException();
	}

	public void SetDamage(float health)
	{
		throw new System.NotImplementedException();
	}
}
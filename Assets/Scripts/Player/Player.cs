using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
	[SerializeField] float maxHP;
	float currentHP;

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
		currentHP = maxHP;
		StartCoroutine(DebugHealth());
	}

	private void Update()
	{
		logic.Logic();
	}

	public void SetHealth(float health)
	{
		currentHP = health;
	}

	public float GetHealth()
	{
		return currentHP;
	}

	IEnumerator DebugHealth()
	{
		while (true)
		{
			Debug.Log("My health is: " + currentHP);
			yield return new WaitForSeconds(5);
		}
	}
}
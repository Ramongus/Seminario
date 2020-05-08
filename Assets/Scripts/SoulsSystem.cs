using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulsSystem : MonoBehaviour
{
	[SerializeField] Text soulsText;

	int soulsInLevel;
	int soulsCompleted;


	public int TotalSouls
	{
		get
		{
			return soulsInLevel;
		}
		private set
		{
			soulsInLevel = value;
		}
	}
	public int SoulsCompleted
	{
		get
		{
			return soulsCompleted;
		}
		private set
		{
			soulsCompleted = value;
		}
	}

	private void Awake()
	{
		TotalSouls = FindObjectsOfType<BattleSystem>().Length;
		EventsManager.SuscribeToEvent("SoulCompleted", OnSoulCompleted);
		UpdateSouls();
	}

	private void OnSoulCompleted(object[] parameters)
	{
		SoulsCompleted++;
		UpdateSouls();
	}

	void UpdateSouls()
	{
		soulsText.text = SoulsCompleted + "/" + TotalSouls;
	}
}

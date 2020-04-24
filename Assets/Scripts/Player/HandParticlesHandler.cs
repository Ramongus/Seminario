using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandParticlesHandler : MonoBehaviour
{
	[SerializeField] GameObject[] handsParticle;
	[SerializeField] int startIndex;
	int index;
	GameObject currentParticle;

	private void Awake()
	{
		index = startIndex;
		currentParticle = handsParticle[index];

		SetParticleActive(currentParticle);

		EventsManager.SuscribeToEvent("NextHabilitie", NextHabilitie);
		EventsManager.SuscribeToEvent("PreviousHabilitie", PreviousHabilitie);
	}

	public void NextHabilitie(params object[] parameters)
	{
		index++;
		CheckIndexValue();
		SetParticleActive(handsParticle[index]);
	}

	public void PreviousHabilitie(params object[] parameters)
	{
		index--;
		CheckIndexValue();
		SetParticleActive(handsParticle[index]);
	}

	private void CheckIndexValue()
	{
		if(index >= handsParticle.Length)
		{
			index = 0;
		}
		else if(index < 0)
		{
			index = handsParticle.Length - 1;
		}
	}

	private void SetParticleActive(GameObject particle)
	{
		currentParticle.SetActive(false);
		currentParticle = particle;
		currentParticle.SetActive(true);
	}
}

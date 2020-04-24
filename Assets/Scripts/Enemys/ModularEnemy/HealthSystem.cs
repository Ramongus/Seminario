using UnityEngine;
using System;

public abstract class HealthSystem : MonoBehaviour
{
	public abstract event EventHandler OnHealthChange;
	public abstract void Sethealth(float health);
	public abstract float GetHealth();
}

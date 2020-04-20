using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
	[SerializeField] Image healthBar;
	[SerializeField] float maxHP;
	[SerializeField] float aimSensitivity;
	[SerializeField] Transform aimPointer;
	[SerializeField] float movementSpeed;
	[SerializeField] List<AbstractAbilities> myHabilities;

	PlayerView _view;
	PlayerModel _model;
	PlayerController _controller;

	private void Awake()
	{
		_view = new PlayerView(GetComponent<Animator>(), healthBar);
		_model = new PlayerModel(transform, movementSpeed, aimPointer, aimSensitivity, _view, maxHP, myHabilities);
		_controller = new PlayerController(_model);
	}

	public float GetHealth()
	{
		return _model.GetCurrentHP();
	}

	public void SetHealth(float health)
	{
		_model.SetHealth(health);
	}
}

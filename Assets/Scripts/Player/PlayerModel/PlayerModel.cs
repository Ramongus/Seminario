using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : ICastAbilities
{
	//Movement data
	PlayerRotationUpdater rotationUpdater;
	Transform _transform;
	float movementSpeed;

	//Aim data
	Transform aimPointer;
	float aimSensitivity;

	PlayerController _controller;

	Vector3 _currentDir;

	PlayerView _view;

	float _maxHP;
	float _currentHp;

	List<AbstractAbilities> _abilities;

	public PlayerModel(Transform playerT, float movSpeed, Transform aimPointer, float aimSensitivity, PlayerView view, float maxHp, List<AbstractAbilities> playerAbilities)
	{
		_abilities = new List<AbstractAbilities>(playerAbilities);
		new AbilitiesManager(_abilities, this);

		_transform = playerT;
		movementSpeed = movSpeed;
		this.aimPointer = aimPointer;
		this.aimSensitivity = aimSensitivity;

		_maxHP = maxHp;
		_currentHp = _maxHP;

		rotationUpdater = new PlayerRotationUpdater(aimPointer, _transform);

		_view = view;
	}

	//Model
	public void BaseMovement(Vector3 axis)
	{
		rotationUpdater.UpdateRotation();
		
		//ACA HACEMOS QUE SI ESTA YENDO PARA ATRAS VAYA MAS LENTO
		float proyectionAxisOnGoingBackDir = Vector3.Dot(axis, -_transform.forward);
		if (proyectionAxisOnGoingBackDir > 0)
			axis -= (axis * proyectionAxisOnGoingBackDir)/2;
		//ACA TERMINA

		_transform.position += axis * movementSpeed * Time.deltaTime;
		Vector3 axisConverted = GetAxisConvertedToPlayerFowardReferece(axis, _transform.forward);
		_currentDir = axisConverted;
		_view.UpdateMovementAnimation(_currentDir);
	}

	//Toma las axis obtenida y la transforma para que sea la direccion real in game del player respescto de su foward
	private Vector3 GetAxisConvertedToPlayerFowardReferece(Vector3 movAxis, Vector3 forward)
	{
		float angleOfForward = Vector3.Angle(Vector3.forward, forward);
		if (forward.x < 0)
			angleOfForward *= -1;

		Vector3 rotatedVector = Quaternion.Euler(0, -angleOfForward, 0) * movAxis;
		return rotatedVector;
	}

	public Transform GetAimPointer()
	{
		return aimPointer;
	}
	
	public float GetAimSensitivity()
	{
		return aimSensitivity;
	}

	public Vector3 GetCurrentDir()
	{
		return _currentDir;
	}

	public float GetMaxHp()
	{
		return _maxHP;
	}

	public float GetCurrentHP()
	{
		return _currentHp;
	}

	public void SetHealth(float health)
	{
		if (health >= _maxHP)
			_currentHp = _maxHP;
		else if (health <= 0)
			_currentHp = 0;
		else
			_currentHp = health;

		_view.UpdateHealthBar(_currentHp / _maxHP);
	}
	public Vector3 GetPosition()
	{
		return _transform.position;
	}
}

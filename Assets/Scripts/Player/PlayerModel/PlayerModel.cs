﻿using System;
using System.Collections;
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

	bool isDashing;
	float dashDuration;
	float dashDistance;
	float dashTimer;
	Vector3 dashInitialPosition;
	Vector3 dashFinalPosition;

	Rigidbody _rigi;

	LayerMask dashRayMask;

	Collider playerCol;

	GameObject mesh;

	GameObject dashTrailParticles;

	public PlayerModel(Transform playerT, float movSpeed, Transform aimPointer, float aimSensitivity, PlayerView view, float maxHp, List<AbstractAbilities> playerAbilities, float dashDuration, float dashDistance, LayerMask dashRayMask, GameObject mesh, GameObject dashTrailParticles)
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

		EventsManager.SuscribeToEvent("Dash", Dash);
		isDashing = false;
		this.dashDuration = dashDuration;
		this.dashDistance = dashDistance;

		_rigi = _transform.gameObject.GetComponent<Rigidbody>();
		this.dashRayMask = dashRayMask;

		this.mesh = mesh;
		playerCol = _transform.gameObject.GetComponent<Collider>();
		this.dashTrailParticles = dashTrailParticles;
	}

	//Model
	public void BaseMovement(Vector3 axis)
	{
		if (!isDashing)
		{
			playerCol.enabled = true;
			mesh.SetActive(true);//Deberia estar en el view
			dashTrailParticles.SetActive(false);//Deberia estar en el view

			rotationUpdater.UpdateRotation();
		
			//ACA HACEMOS QUE SI ESTA YENDO PARA ATRAS VAYA MAS LENTO
			float proyectionAxisOnGoingBackDir = Vector3.Dot(axis, -_transform.forward);
			if (proyectionAxisOnGoingBackDir > 0)
				axis -= (axis * proyectionAxisOnGoingBackDir)/2;
			//ACA TERMINA

			//Movement by transform DEPRECATED
			//_transform.position += axis * movementSpeed * Time.deltaTime;

			_rigi.velocity = axis * movementSpeed;

			//_rigi.velocity += axis * movementSpeed;
			//_rigi.velocity = Vector3.ClampMagnitude(_rigi.velocity, movementSpeed);

			Vector3 axisConverted = GetAxisConvertedToPlayerFowardReferece(axis, _transform.forward);
			_currentDir = axisConverted;
			_view.UpdateMovementAnimation(_currentDir);
		}
		else
		{
			float timeBooster = (dashFinalPosition - dashInitialPosition).magnitude / dashDistance;
			dashTimer -= Time.deltaTime / timeBooster;
			_transform.position = Vector3.Lerp(dashInitialPosition, dashFinalPosition, (dashDuration - dashTimer) / dashDuration);
			if(dashTimer <= 0)
			{
				isDashing = false;
			}

		}
	}

	public void Dash(params object[] parameters)
	{
		if (!isDashing)
		{
			playerCol.enabled = false;
			mesh.SetActive(false);//Deberia estar en el view
			dashTrailParticles.SetActive(true);//Deberia estar en el view

			isDashing = true;
			dashTimer = dashDuration;
			dashInitialPosition = _transform.position;

			RaycastHit hit;
			if (Physics.Raycast(_transform.position, _transform.forward, out hit, dashDistance, dashRayMask))
			{
				dashFinalPosition = hit.point;
			}
			else
			{
				dashFinalPosition = dashInitialPosition + _transform.forward * dashDistance;
			}
		}
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
		{
			_currentHp = 0;
			_view.DieAnimation();
			EventsManager.TriggerEvent("OnPlayerDie");
		}
		else
			_currentHp = health;

		_view.UpdateHealthBar(_currentHp / _maxHP);
	}
	public Vector3 GetPosition()
	{
		return _transform.position;
	}
}

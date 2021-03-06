﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
public class PlayerModel : ICastAbilities
{
	Player _player;

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
	float dashCooldown;
	float dashCooldownTimer;
	Vector3 dashInitialPosition;
	Vector3 dashFinalPosition;

	Rigidbody _rigi;

	LayerMask dashRayMask;

	Collider playerCol;

	GameObject mesh;

	ParticleSystem dashTrailParticles;
	ParticleSystem.MinMaxCurve initialDashParticleRateOverDistance;

	Vignette vignette;
	ColorParameter vignetteInitialColor;
	FloatParameter vignetteInitialIntensity;

	bool isCastingSpell;
	bool isFalling;
	bool isDead;

	float fallingTimer;
	float fallingMaxTime;

	public PlayerModel(Transform playerT, float movSpeed, Transform aimPointer, float aimSensitivity, PlayerView view, float maxHp, List<AbstractAbilities> playerAbilities, float dashDuration, float dashDistance, float dashCooldown, LayerMask dashRayMask, GameObject mesh, ParticleSystem dashTrailParticles, Player player)
	{
		EventsManager.SuscribeToEvent("StartCastingSpell", StartCastingSpell);
		EventsManager.SuscribeToEvent("StopCastingSpell", StopCastingSpell);

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
		this.dashCooldown = dashCooldown;
		dashCooldownTimer = 0;

		_rigi = _transform.gameObject.GetComponent<Rigidbody>();
		this.dashRayMask = dashRayMask;

		this.mesh = mesh;
		playerCol = _transform.gameObject.GetComponent<Collider>();
		this.dashTrailParticles = dashTrailParticles;
		var emission = dashTrailParticles.emission;
		initialDashParticleRateOverDistance = emission.rateOverDistance;

		Camera.main.gameObject.GetComponent<PostProcessVolume>().profile.TryGetSettings(out vignette);
		vignetteInitialColor = vignette.color;
		vignetteInitialIntensity = vignette.intensity;

		_player = player;
		fallingMaxTime = _player.MaxFallingTime;
	}

	//Model
	public void BaseMovement(Vector3 axis)
	{
		if (isDead) return;
		if (!isDashing)
		{
			mesh.SetActive(true);//Deberia estar en el view
			var emission = dashTrailParticles.emission; //Deberia estar en el view
			emission.rateOverDistance = 0;

			if (!CheckFloor()) return;

			rotationUpdater.UpdateRotation();
			if (isCastingSpell) return;
			dashCooldownTimer -= Time.deltaTime;


			vignette.intensity.Override(0.2f);
			vignette.color.Override(new Color(0,0,0,1));

		
			//ACA HACEMOS QUE SI ESTA YENDO PARA ATRAS VAYA MAS LENTO
			float proyectionAxisOnGoingBackDir = Vector3.Dot(axis, -_transform.forward);
			if (proyectionAxisOnGoingBackDir > 0)
				axis -= (axis * proyectionAxisOnGoingBackDir)/2;
			//ACA TERMINA

			//Movement by transform DEPRECATED
			//_transform.position += axis * movementSpeed * Time.deltaTime;

			Vector3 movementXZ = axis * movementSpeed;
			_rigi.velocity = new Vector3(movementXZ.x, _rigi.velocity.y, movementXZ.z);

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
				dashCooldownTimer = dashCooldown;
				isDashing = false;
				playerCol.enabled = true;
				_rigi.isKinematic = false;
			}

		}
	}

	public void Dash(params object[] parameters)
	{
		if (!isDashing && dashCooldownTimer <= 0 && !isCastingSpell && CheckFloor() && !isDead)
		{
			Vector3 dashDir = _transform.forward;
			playerCol.enabled = false;
			_rigi.isKinematic = true;
			mesh.SetActive(false);//Deberia estar en el view
			var emission = dashTrailParticles.emission; //Deberia estar en el view
			emission.rateOverDistance = initialDashParticleRateOverDistance;

			vignette.intensity.Override(0.4f);
			vignette.color.Override(new Color(0.5f, 0.1f, 0.9f, 1));

			isDashing = true;
			dashTimer = dashDuration;
			dashInitialPosition = _transform.position;

			RaycastHit hit;

			if (Physics.Raycast(_transform.position, dashDir, out hit, dashDistance, dashRayMask))
			{
				dashFinalPosition = hit.point;
			}
			else
			{
				dashFinalPosition = dashInitialPosition + dashDir * dashDistance;
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
		if (health < _currentHp) _view.DamagedVignetteAnimation();

		if (health >= _maxHP)
			_currentHp = _maxHP;
		else if (health <= 0)
		{
			SetDieValues();
			if (_currentHp > 0)
				EventsManager.TriggerEvent("RestartLevel");
			_currentHp = 0;
		}
		else
			_currentHp = health;

		_view.UpdateHealthBar(_currentHp / _maxHP);
	}
	public Vector3 GetPosition()
	{
		return _transform.position;
	}

	public void StartCastingSpell(params object[] parameters)
	{
		BaseMovement(Vector3.zero);
		isCastingSpell = true;
	}

	public void StopCastingSpell(params object[] parameters)
	{
		isCastingSpell = false;
	}

	public bool IsCastingAbilitie()
	{
		return isCastingSpell;
	}

	public bool CheckFloor()
	{
		if(!Physics.Raycast(_transform.position, -_transform.up, _player.FloorCheckDistance, _player.ArenaLayer, QueryTriggerInteraction.Ignore))
		{
			fallingTimer -= Time.deltaTime;
			if(fallingTimer <= 0)
			{
				SetFallingValues();
				if(!isFalling)
					EventsManager.TriggerEvent("RestartLevel");
				isFalling = true;
				return false;
			}
			return true;
		}
		fallingTimer = fallingMaxTime;
		return true;
	}

	public void SetInitialValues()
	{
		isDead = false;
		isFalling = false;
		playerCol.enabled = true;
		SetHealth(_maxHP);
		_view.SetIdleAnimation();
		_rigi.velocity = Vector3.zero;
		_view.SetIdleAnimation();
	}

	public void SetDieValues()
	{
		isDead = true;
		playerCol.enabled = false;
		_rigi.isKinematic = true;
		_view.DieAnimation();
	}

	public void SetFallingValues()
	{
		_rigi.velocity = new Vector3(0, _rigi.velocity.y, 0);
		playerCol.enabled = false;
		_view.FallingAnimation();
	}
}

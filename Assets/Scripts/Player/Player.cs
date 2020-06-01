using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
	[SerializeField] Animator canvasAnimator;
	[SerializeField] Image healthBar;
	[SerializeField] float maxHP;
	[SerializeField] float aimSensitivity;
	[SerializeField] Transform aimPointer;
	[SerializeField] float movementSpeed;
	[SerializeField] List<AbstractAbilities> myHabilities;
	[SerializeField] float dashDuration;
	[SerializeField] float dashDistance;
	[SerializeField] float dashCooldown;
	[SerializeField] LayerMask rayMaskLayers;
	[SerializeField] GameObject mesh;
	[SerializeField] ParticleSystem dashTrailParticle;

	[SerializeField] float floorCheckDistance;
	public float FloorCheckDistance
	{
		get
		{
			return floorCheckDistance;
		} 
	}

	[SerializeField] LayerMask arenaLayer;
	public LayerMask ArenaLayer
	{
		get
		{
			return arenaLayer;
		}
	}

	PlayerModel _model;
	PlayerView _view;
	PlayerController _controller;

	private void Awake()
	{
		_view = new PlayerView(GetComponent<Animator>(), healthBar, canvasAnimator);
		_model = new PlayerModel(transform, movementSpeed, aimPointer, aimSensitivity, _view, maxHP, myHabilities, dashDuration, dashDistance, dashCooldown, rayMaskLayers, mesh, dashTrailParticle, this);
		_controller = new PlayerController(_model);
		EventsManager.SuscribeToEvent("OnPlayerDie", TurnOffComponents);
		EventsManager.SuscribeToEvent("PlayerResurrect", TurnOnComponents);
	}

	public float GetHealth()
	{
		return _model.GetCurrentHP();
	}

	public void SetHealth(float health)
	{
		_model.SetHealth(health);
	}

	public void TurnOffComponents(params object[] parameters)
	{
		Collider[] colliders = GetComponents<Collider>();
		foreach (Collider collider in colliders)
		{
			collider.enabled = false;
		}

		Rigidbody rigi = GetComponent<Rigidbody>();
		rigi.isKinematic = true;

		//_controller = null;
	}

	public void TurnOnComponents(params object[] parameters)
	{
		Collider[] colliders = GetComponents<Collider>();
		foreach (Collider collider in colliders)
		{
			collider.enabled = true;
		}

		Rigidbody rigi = GetComponent<Rigidbody>();
		rigi.isKinematic = false;

		//_controller = new PlayerController(_model);
	}

	public void SetInitialValues() { _model.SetInitialValues(); }

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.forward * dashDistance + transform.position);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, -transform.up * FloorCheckDistance + transform.position);
	}
}

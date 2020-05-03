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
	[SerializeField] float dashDuration;
	[SerializeField] float dashDistance;
	[SerializeField] LayerMask rayMaskLayers;
	[SerializeField] GameObject mesh;
	[SerializeField] GameObject dashTrailParticle;

	PlayerView _view;
	PlayerModel _model;
	PlayerController _controller;

	private void Awake()
	{
		_view = new PlayerView(GetComponent<Animator>(), healthBar);
		_model = new PlayerModel(transform, movementSpeed, aimPointer, aimSensitivity, _view, maxHP, myHabilities, dashDuration, dashDistance, rayMaskLayers, mesh, dashTrailParticle);
		_controller = new PlayerController(_model);
		EventsManager.SuscribeToEvent("OnPlayerDie", TurnOffComponents);
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

		_controller = null;
	}
}

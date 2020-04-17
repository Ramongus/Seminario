using System;
using UnityEngine;

public class PlayerModel
{
	//Movement data
	PlayerMovementController movementController;
	RotationUpdater rotationUpdater;
	Transform playerTransform;
	float movementSpeed;

	//Aim data
	Transform aimPointer;
	float aimSensitivity;

	//HabilitiesController data
	HablilitiesController habilitiesController;

	Animator playerAnimator;


	PlayerController _controller;

	public PlayerModel(Transform playerT, float movSpeed, Transform aimPointer, float aimSensitivity, Animator playerAnimator)
	{
		playerTransform = playerT;
		movementSpeed = movSpeed;
		this.aimPointer = aimPointer;
		this.aimSensitivity = aimSensitivity;

		rotationUpdater = new RotationUpdater(aimPointer, playerTransform);

		this.playerAnimator = playerAnimator;

		_controller = new PlayerController(this);

		EventsManager.SuscribeToEvent("FireHabilitie", ThrowSpellAnimation);
	}

	public void ThrowSpellAnimation(params object[] parameters)
	{
		playerAnimator.SetTrigger("Attack");
	}

	//Model
	public void BaseMovement(Vector3 axis)
	{
		rotationUpdater.UpdateRotation();
		float proyectionAxisOnGoingBackDir = Vector3.Dot(axis, -playerTransform.forward);
		if (proyectionAxisOnGoingBackDir > 0)
			axis -= (axis * proyectionAxisOnGoingBackDir)/2;
		playerTransform.position += axis * movementSpeed * Time.deltaTime;
		UpdateAnimator(axis);
	}

	//Esta funcion deberia estar en la view
	private void UpdateAnimator(Vector3 movAxis)
	{
		if (movAxis != Vector3.zero)
			playerAnimator.SetFloat("Speed", 1);
		else
		{
			playerAnimator.SetFloat("Speed", 0);
			return;
		}

		Vector3 axisConverted = GetAxisInComparisonOfPlayerFoward(movAxis, playerTransform.forward);

		playerAnimator.SetFloat("zAxis", axisConverted.z);
		playerAnimator.SetFloat("xAxis", axisConverted.x);
	}

	//Toma las axis obtenida y la transforma para que sea la direccion real in game del player respescto de su foward
	private Vector3 GetAxisInComparisonOfPlayerFoward(Vector3 movAxis, Vector3 forward)
	{
		float angleOfForward = Vector3.Angle(Vector3.forward, forward);
		if (forward.x < 0)
			angleOfForward *= -1;

		Vector3 rotatedVector = Quaternion.Euler(0, -angleOfForward, 0) * movAxis;
		return rotatedVector;
	}

	//Habilities Inputs -- EN EL CONTROLLER
	private void HabilitiesInputs()
	{
		habilitiesController.ManageHabilities();
	}

	public Transform GetAimPointer()
	{
		return aimPointer;
	}

	public float GetAimSensitivity()
	{
		return aimSensitivity;
	}
}

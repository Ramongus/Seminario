using System;
using UnityEngine;

public class PlayerLogic
{
	/*
	 * Manejo de inputs.
	 *	-MovementController
	 *		.BaseMovement
	 *		
	 *	-HabilitiesController
	 *		.Aim
	 *		.Habilitie Selection
	 *		.Habilitie Caster
	 */

	//Movement data
	SimpleController movementController;
	RotationUpdater rotationUpdater;
	Transform playerTransform;
	float movementSpeed;

	//Aim data
	Transform aimPointer;
	float aimSensitivity;

	//HabilitiesController data
	HablilitiesController habilitiesController;

	Animator playerAnimator;


	public PlayerLogic(Transform playerT, float movSpeed, Transform aimPointer, float aimSensitivity, Animator playerAnimator)
	{
		playerTransform = playerT;
		movementSpeed = movSpeed;
		this.aimPointer = aimPointer;
		this.aimSensitivity = aimSensitivity;

		movementController = new SimpleController();
		rotationUpdater = new RotationUpdater(aimPointer, playerTransform);
		habilitiesController = new HablilitiesController(aimSensitivity, aimPointer);

		this.playerAnimator = playerAnimator;

		EventsManager.SuscribeToEvent("FireSpell", ThrowSpellAnimation);
	}


	public void Logic()
	{
		BaseMovement();
		HabilitiesInputs();
	}

	public void ThrowSpellAnimation(params object[] parameters)
	{
		playerAnimator.SetTrigger("Attack");
	}
	//BaseMovement methods
	private void BaseMovement()
	{
		Vector3 axis = movementController.GetMovementAxis();
		UpdateAnimator(axis);
		playerTransform.position += axis * movementSpeed * Time.deltaTime;
		rotationUpdater.UpdateRotation();
	}

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

	private Vector3 GetAxisInComparisonOfPlayerFoward(Vector3 movAxis, Vector3 forward)
	{
		//float angleToRotate = Vector3.Angle(movAxis, forward);
		/*
		float angleOfMovAxis = Vector3.Angle(Vector3.forward, movAxis);
		if (movAxis.x < 0)
			angleOfMovAxis *= -1;
		*/

		float angleOfForward = Vector3.Angle(Vector3.forward, forward);
		if (forward.x < 0)
			angleOfForward *= -1;

		Vector3 rotatedVector = Quaternion.Euler(0, -angleOfForward, 0) * movAxis;
		return rotatedVector;
	}

	//Habilities Inputs
	private void HabilitiesInputs()
	{
		habilitiesController.ManageHabilities();
	}
}

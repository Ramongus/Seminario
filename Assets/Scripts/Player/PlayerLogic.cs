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


	public PlayerLogic(Transform playerT, float movSpeed, Transform aimPointer, float aimSensitivity)
	{
		playerTransform = playerT;
		movementSpeed = movSpeed;
		this.aimPointer = aimPointer;
		this.aimSensitivity = aimSensitivity;

		movementController = new SimpleController();
		rotationUpdater = new RotationUpdater(aimPointer, playerTransform);
		habilitiesController = new HablilitiesController(aimSensitivity, aimPointer);

	}

	public void Logic()
	{
		BaseMovement();
		HabilitiesInputs();
	}

	//BaseMovement methods
	private void BaseMovement()
	{
		playerTransform.position += movementController.GetMovementAxis() * movementSpeed * Time.deltaTime;
		rotationUpdater.UpdateRotation();
	}

	//Habilities Inputs
	private void HabilitiesInputs()
	{
		habilitiesController.ManageHabilities();
	}
}

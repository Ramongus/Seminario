﻿using UnityEngine;

public class PlayerController : IUpdate
{
	PlayerModel _playerModel;
	PlayerMovementController movementController;
	PlayerAbilitiesController habilitiesController;

	public PlayerController(PlayerModel model)
	{
		_playerModel = model;
		movementController = new PlayerMovementController();
		habilitiesController = new PlayerAbilitiesController(_playerModel.GetAimSensitivity(), _playerModel.GetAimPointer());
		EventsManager.TriggerEvent("SuscribeToUpdateManager", this);
	}

	public void CheckInputs()
	{
		Vector3 axis = movementController.GetMovementAxis();
		_playerModel.BaseMovement(axis);
		habilitiesController.ManageHabilities();
	}

	public void MyUpdate()
	{
		CheckInputs();
	}
}
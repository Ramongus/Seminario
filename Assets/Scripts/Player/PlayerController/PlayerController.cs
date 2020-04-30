using UnityEngine;

public class PlayerController : IUpdate
{
	PlayerModel _playerModel;
	PlayerMovementController movementController;
	PlayerAbilitiesController abilitiesController;
	PlayerDashController dashController;

	public PlayerController(PlayerModel model)
	{
		_playerModel = model;
		movementController = new PlayerMovementController();
		abilitiesController = new PlayerAbilitiesController(_playerModel.GetAimSensitivity(), _playerModel.GetAimPointer());
		dashController = new PlayerDashController();
		EventsManager.TriggerEvent("SuscribeToUpdateManager", this);
	}

	public void MyUpdate()
	{
		CheckInputs();
	}

	public void CheckInputs()
	{
		Vector3 axis = movementController.GetMovementAxis();
		_playerModel.BaseMovement(axis);
		abilitiesController.ManageAbilities();
		dashController.CheckDash();
	}
}

using UnityEngine;

public class PlayerAbilitiesController
{
	PlayerPointerController aimController;
	PlayerAbilitieSelectorController abilitiesSelector;
	PlayerAbilitieActivator spellCaster;
	PlayerModel model;

	public PlayerAbilitiesController(float aimSensitivity, Transform aimPointer, PlayerModel model, bool isJoystick = false)
	{
		aimController = new PlayerPointerController(aimSensitivity, aimPointer, isJoystick);
		abilitiesSelector = new PlayerAbilitieSelectorController();
		spellCaster = new PlayerAbilitieActivator(aimPointer);
		this.model = model;
	}

	public void ManageAbilities()
    {
		aimController.AimUpdate();
		abilitiesSelector.UpdateChangeHabilitie();
		spellCaster.ActivateHabilitie();
    }
}
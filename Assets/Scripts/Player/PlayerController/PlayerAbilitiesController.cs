using UnityEngine;

public class PlayerAbilitiesController
{
	PlayerPointerController aimController;
	PlayerAbilitieSelectorController habilitiesSelector;
	PlayerAbilitieActivator spellCaster;

	public PlayerAbilitiesController(float aimSensitivity, Transform aimPointer, bool isJoystick = false)
	{
		aimController = new PlayerPointerController(aimSensitivity, aimPointer, isJoystick);
		habilitiesSelector = new PlayerAbilitieSelectorController();
		spellCaster = new PlayerAbilitieActivator(aimPointer);
	}

	public void ManageHabilities()
    {
		aimController.AimUpdate();
		habilitiesSelector.UpdateChangeHabilitie();
		spellCaster.ActivateHabilitie();
    }
}
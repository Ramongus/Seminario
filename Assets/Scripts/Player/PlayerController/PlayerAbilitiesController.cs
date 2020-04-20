using UnityEngine;

public class PlayerAbilitiesController
{
	PlayerPointerController aimController;
	PlayerAbilitieSelectorController abilitiesSelector;
	PlayerAbilitieActivator spellCaster;

	public PlayerAbilitiesController(float aimSensitivity, Transform aimPointer, bool isJoystick = false)
	{
		aimController = new PlayerPointerController(aimSensitivity, aimPointer, isJoystick);
		abilitiesSelector = new PlayerAbilitieSelectorController();
		spellCaster = new PlayerAbilitieActivator(aimPointer);
	}

	public void ManageAbilities()
    {
		aimController.AimUpdate();
		abilitiesSelector.UpdateChangeHabilitie();
		spellCaster.ActivateHabilitie();
    }
}
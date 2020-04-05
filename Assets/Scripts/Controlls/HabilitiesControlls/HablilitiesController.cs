using UnityEngine;

public class HablilitiesController
{
	AimPointerController aimController;
	HabilitiesSelectorController habilitiesSelector;
	HabilitieActivator spellCaster;

	public HablilitiesController(float aimSensitivity, Transform aimPointer, bool isJoystick)
	{
		aimController = new AimPointerController(aimSensitivity, aimPointer, isJoystick);
		habilitiesSelector = new HabilitiesSelectorController();
		spellCaster = new HabilitieActivator(aimPointer);
	}

	public void ManageHabilities()
    {
		aimController.AimUpdate();
		habilitiesSelector.UpdateChangeHabilitie();
		spellCaster.ActivateHabilitie();
    }
}

using UnityEngine;

public class HablilitiesController
{
	AimPointerController aimController;
	HabilitiesSelectorController habilitiesSelector;
	HabilitieActivator spellCaster;
	DashController dashController;

	public HablilitiesController(float aimSensitivity, Transform aimPointer, bool isJoystick)
	{
		aimController = new AimPointerController(aimSensitivity, aimPointer, isJoystick);
		habilitiesSelector = new HabilitiesSelectorController();
		spellCaster = new HabilitieActivator(aimPointer);
		dashController = new DashController(0.8f, aimPointer);
	}

	public void ManageHabilities()
    {
		aimController.AimUpdate();
		habilitiesSelector.UpdateChangeHabilitie();
		spellCaster.ActivateHabilitie();
		dashController.Dash();
    }
}

public class DashController
{
	float timeDashing;
	Transform aimPointer;

	public DashController(float timeDashing, Transform aimPointer)
	{
		this.timeDashing = timeDashing;
		this.aimPointer = aimPointer;
	}

	public void Dash()
	{
		if (Input.GetButtonUp("Dash"))
		{
			EventsManager.TriggerEvent("Dash", aimPointer.position, timeDashing);
		}
	}
}
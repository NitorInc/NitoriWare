using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryCompilationStage : CompilationStage
{
	[SerializeField]
	private Transform disable;

	public override void onStageStart()
	{
		base.onStageStart();
        updateCursorVisible();
	}

	void Update()
	{
		updateCursorVisible();
	}

	void LateUpdate()
	{
		updateCursorVisible();
	}

	void updateCursorVisible()
	{
		if (MicrogameController.instance != null)
            Cursor.visible = MicrogameController.instance.getTraits().controlScheme == global::Microgame.ControlScheme.Mouse
				&& !MicrogameController.instance.getTraits().GetHideCursor(MicrogameController.instance.Session);
		else
			Cursor.visible = true;
	}
}

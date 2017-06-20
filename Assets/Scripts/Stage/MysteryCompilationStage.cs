using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryCompilationStage : CompilationStage
{
	[SerializeField]
	private Transform disable;

	void Start()
	{
		disable.gameObject.SetActive(false);
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
			Cursor.visible = MicrogameController.instance.getTraits().controlScheme == MicrogameTraits.ControlScheme.Mouse
				&& !MicrogameController.instance.getTraits().hideCursor;
		else
			Cursor.visible = true;
	}
}

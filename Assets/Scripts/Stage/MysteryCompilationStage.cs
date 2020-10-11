using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage/Mystery Compilation Stage")]
public class MysteryCompilationStage : CompilationStage
{
	[SerializeField]
	private Transform disable;

    public override void InitStage(int seed = 0)
    {
        base.InitStage(seed);
        updateCursorVisible();
	}

	// TODO this used to be a gameobject so, fix this

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
                && !MicrogameController.instance.session.GetHideCursor();
        else
            Cursor.visible = true;
	}
}

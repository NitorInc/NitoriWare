using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardCompilationStage : CompilationStage
{
    public int progressionScoreThreshold;

	public override void onStageStart()
	{
		base.onStageStart();
		roundsCompleted = 2;
	}

	public override int getMicrogameDifficulty(Stage.Microgame microgame, int num) => 3;

	public override int getMaxLife() => 1;

    public override string getExitScene()
    {
        if (PrefsHelper.getProgress() < PrefsHelper.GameProgress.AllCompilationComplete && PrefsHelper.getHighScore(gameObject.scene.name) >= progressionScoreThreshold)
        {
            PrefsHelper.setProgress(PrefsHelper.GameProgress.AllCompilationComplete);
            GameMenu.subMenu = GameMenu.SubMenu.Credits;
            return "NitoriSplash";
        }
        else
            return base.getExitScene();
    }

}

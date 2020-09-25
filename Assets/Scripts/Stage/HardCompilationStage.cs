using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Stage/Hard Compilation Stage")]
public class HardCompilationStage : CompilationStage
{
    public int progressionScoreThreshold;

	//public override void onStageStart(StageController stageController)
	//{
	//	base.onStageStart(stageController);
	//	roundsCompleted = 2;
	//}



	public override int getMaxLife()
	{
		return 1;
	}

    public override string getExitScene()
    {
        if (PrefsHelper.getProgress() < PrefsHelper.GameProgress.AllCompilationComplete && PrefsHelper.getHighScore(name) >= progressionScoreThreshold)
        {
            PrefsHelper.setProgress(PrefsHelper.GameProgress.AllCompilationComplete);
            GameMenu.subMenu = GameMenu.SubMenu.Credits;
            return "NitoriSplash";
        }
        else
            return base.getExitScene();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame/Single Scene")]
public class MicrogameSingleScene : Microgame
{
    public override bool SceneDeterminesDifficulty => false;
    
    class Session : MicrogameSession
    {
        public override string SceneName => microgame.microgameId;

        public Session(Microgame microgame, StageController player, int difficulty, bool debugMode)
            : base(microgame, player, difficulty, debugMode)
        {
        }
    }

    public override Microgame.MicrogameSession CreateSession(StageController player, int difficulty, bool debugMode = false)
    {
        return new Session(this, player, difficulty, debugMode);
    }
}


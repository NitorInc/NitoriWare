using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MicrogameSession
{
    public Microgame microgame { get; private set; }

    public int Difficulty { get; private set; }

    public bool Victory { get; set; }

    public float VictoryVoiceDelay { get; set; }

    public float FailureVoiceDelay { get; set; }


    public virtual bool HideCursor => microgame.hideCursorDefault;

    public virtual CursorLockMode cursorLockMode => microgame.cursorLockStateDefault;

    public virtual string GetNonLocalizedCommand => microgame.commandDefault;

    public virtual string GetLocalizedCommand(MicrogameSession session) =>
        TextHelper.getLocalizedText($"microgame.{microgame.MicrogameId}.command", GetNonLocalizedCommand);

    public virtual AudioClip MusicClip => microgame.MusicClipDefault;

    public virtual string SceneName => microgame.MicrogameId + Difficulty.ToString();

    public virtual int GetDifficultyFromScene(string sceneName) => int.Parse(sceneName.Last().ToString());

    public virtual AnimatorOverrideController CommandAnimatorOverride => microgame.CommandAnimatorOverrideDefault;

    
    /// <summary>
    /// Only meant to be called by Microgame class, use Microgame.CreateSession to generate one for gameplay
    /// </summary>
    public  MicrogameSession(Microgame microgame, int difficulty, bool debugMode = false)
    {
        this.microgame = microgame;
        Difficulty = difficulty;
        Victory = microgame.defaultVictory;
        VictoryVoiceDelay = microgame.VictoryVoiceDelayDefault;
        FailureVoiceDelay = microgame.FailureVoiceDelayDefault;
    }
}

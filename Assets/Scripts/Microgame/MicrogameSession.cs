using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MicrogameSession : IDisposable
{
    static List<MicrogameSession> activeSessions;
    public static ReadOnlyCollection<MicrogameSession> ActiveSessions =>
        (activeSessions != null  ? activeSessions : new List<MicrogameSession>()).AsReadOnly();
    
    public Microgame microgame { get; private set; }

    public StageController microgamePlayer { get; private set; }

    public int Difficulty { get; private set; }

    public bool Victory { get; set; }

    public bool VictoryWasDetermined { get; set; } = false;

    public float VictoryVoiceDelay { get; set; }

    public float FailureVoiceDelay { get; set; }

    public SessionState State { get; set; }
    public enum SessionState
    {
        Loading,
        Playing,
        Unloading
    }


    public virtual bool HideCursor => microgame.hideCursorDefault;

    public virtual CursorLockMode cursorLockMode => microgame.cursorLockStateDefault;

    public virtual string GetNonLocalizedCommand() => microgame.commandDefault;

    public virtual string GetLocalizedCommand() =>
        TextHelper.getLocalizedText($"microgame.{microgame.microgameId}.command", GetNonLocalizedCommand());

    public virtual AudioClip MusicClip => microgame.MusicClipDefault;

    public virtual string SceneName => microgame.microgameId + Difficulty.ToString();

    public virtual AnimatorOverrideController CommandAnimatorOverride => microgame.CommandAnimatorOverrideDefault;

    /// <summary>
    /// Not to be called except from Microgame.CreateSession and inherited classes
    /// If you inherit this class to randomize certain start attributes, randomize them in the constructor
    /// </summary>
    public  MicrogameSession(Microgame microgame, StageController player, int difficulty, bool debugMode)
    {
        this.microgame = microgame;
        Difficulty = difficulty;
        Victory = microgame.defaultVictory;
        VictoryVoiceDelay = microgame.VictoryVoiceDelayDefault;
        FailureVoiceDelay = microgame.FailureVoiceDelayDefault;

        if (activeSessions == null)
            activeSessions = new List<MicrogameSession>();
        activeSessions.Add(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Prevent leaks  :)

        if (mode == LoadSceneMode.Single)
        {
            State = SessionState.Unloading;
            Dispose();
        }
    }

    public void Dispose()
    {
        activeSessions.Remove(this);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}

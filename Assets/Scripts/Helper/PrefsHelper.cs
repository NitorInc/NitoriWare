using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefsHelper
{
    // Increase this when the game is changed enough to warrant resetting high scores
    private const int PrefsVersion = 3;

    private const string VersionKey = "version";
    private const string PreferredLanguageKey = "settings.preferredlanguage";
    private const string VolumeKeyPrefix = "settings.volume.";
    private const string ProgressKey = "save.progress"; //How many gamemodes the player has completed
    private const string HighScorePrefix = "save.highscore.";
    private const string StageUnlockPrefix = "save.unlocked.";
    private const string StageVisitPrefix = "save.visited.";

    private static int volumeTypeCount = 4;

    private static StoredPrefs storedPrefs = loadPrefs();
    private struct StoredPrefs
    {
        public string preferredLanguage;
        public float[] volumes;
    }

    public enum VolumeType
    {
        Master,
        Music,
        SFX,
        Voice
    }

    public enum GameProgress
    {
        Started = 0,
        StoryComplete = 1,
        AllCompilationComplete = 2
    }

    public static void initiate()
    {
        var recordedVersion = PlayerPrefs.GetInt(VersionKey, 0);
        if (recordedVersion != PrefsVersion)
        {
            handleLegacyVersions(recordedVersion);
            PlayerPrefs.SetInt(VersionKey, PrefsVersion);
        }
    }

    static void handleLegacyVersions(int recordedVersion)
    {
        // Check for scores implemented before implementing versions
        if (getProgress() >= GameProgress.StoryComplete) // Pre version 3
        {
            setStageUnlocked("compilation", true);
            setStageUnlocked("compilationfast", getHighScore("compilation") >= 15);
            setStageUnlocked("compilationmystery", getHighScore("compilationfast") >= 10);
            setStageUnlocked("compilationhard", getHighScore("compilationmystery") >= 15);
        }
    }


    private static StoredPrefs loadPrefs()
    {
        StoredPrefs newPrefs = new StoredPrefs();
        newPrefs.preferredLanguage = PlayerPrefs.GetString(PreferredLanguageKey, "");
        newPrefs.volumes = new float[volumeTypeCount];
        for (int i = 0; i < volumeTypeCount; i++)
        {
            newPrefs.volumes[i] = PlayerPrefs.GetFloat(VolumeKeyPrefix + ((VolumeType)i).ToString(), 1f);
        }
        return newPrefs;
    }

    /// <summary>
    /// Returns preferred language
    /// </summary>
    /// <returns></returns>
    public static string getPreferredLanguage()
    {
        return storedPrefs.preferredLanguage;
    }

    /// <summary>
    /// Sets preferred language and saves to player prefs
    /// </summary>
    /// <param name="language"></param>
    public static void setPreferredLanguage(string language)
    {
        storedPrefs.preferredLanguage = language;
        PlayerPrefs.SetString(PreferredLanguageKey, language);
    }

    /// <summary>
    /// Returns raw value of volume for given type, unscaled by Master
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static float GetVolumeSetting(VolumeType type)
    {
        return storedPrefs.volumes[(int)type];
    }

    /// <summary>
    /// Sets volume of given type and saves to PlayerPrefs
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public static void setVolume(VolumeType type, float value)
    {
        storedPrefs.volumes[(int)type] = value;
        PlayerPrefs.SetFloat(VolumeKeyPrefix + type.ToString(), value);
    }

    /// <summary>
    /// Returns how many stages the player has beatsn, from story mode to arcade modes
    /// </summary>
    /// <returns></returns>
    public static GameProgress getProgress()
    {
        if (GameController.instance.ShowcaseMode)
            return GameProgress.AllCompilationComplete;
        else
            return (GameProgress)PlayerPrefs.GetInt(ProgressKey, 0);
    }

    /// <summary>
    /// Sets the amount of stages the player has won
    /// </summary>
    /// <param name="progress"></param>
    public static void setProgress(GameProgress progress)
    {
        PlayerPrefs.SetInt(ProgressKey, (int)progress);
    }
    
    /// <summary>
    /// Returns player's high score for given scene name
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    public static int getHighScore(string stage)
    {
        stage = stage.ToLower();
        var version = PlayerPrefs.GetInt(HighScorePrefix + stage.ToLower() + ".version", 0);

        // Reset score if older version
        if (version < PrefsVersion)
        {
            //setHighScore(stage, 0);
            return 0;
        }
        return PlayerPrefs.GetInt(HighScorePrefix + stage.ToLower(), 0);
    }
    
    /// <summary>
    /// Sets the player's highs core to a new score
    /// </summary>
    /// <param name="stage"></param>
    /// <param name="score"></param>
    public static void setHighScore(string stage, int score)
    {
        stage = stage.ToLower();
        PlayerPrefs.SetInt(HighScorePrefix + stage.ToLower(), score);
        PlayerPrefs.SetInt(HighScorePrefix + stage.ToLower() + ".version", PrefsVersion);
    }

    /// <summary>
    /// Returns whether the stage was ever visited by the player
    /// </summary>
    /// <param name="stage"></param>
    public static bool getVisitedStage(string stage)
    {
        stage = stage.ToLower();
        return PlayerPrefs.GetInt(StageVisitPrefix + stage, 0) > 0;
    }

    /// <summary>
    /// Set whether the player has visited the stage at least once
    /// </summary>
    /// <param name="stage"></param>
    /// <param name="visited"></param>
    public static void setVisitedStage(string stage, bool visited)
    {
        stage = stage.ToLower();
        PlayerPrefs.SetInt(StageVisitPrefix + stage, visited ? 1 : 0);
    }

    /// <summary>
    /// Returns whether the stage was ever Unlocked by the player
    /// </summary>
    /// <param name="stage"></param>
    public static bool isStageUnlocked(string stage)
    {
        stage = stage.ToLower();
        return PlayerPrefs.GetInt(StageUnlockPrefix + stage, 0) > 0;
    }

    /// <summary>
    /// Set whether the player has Unlocked the stage at least once
    /// </summary>
    /// <param name="stage"></param>
    /// <param name="Unlocked"></param>
    public static void setStageUnlocked(string stage, bool Unlocked)
    {
        stage = stage.ToLower();
        PlayerPrefs.SetInt(StageUnlockPrefix + stage, Unlocked ? 1 : 0);
    }
}

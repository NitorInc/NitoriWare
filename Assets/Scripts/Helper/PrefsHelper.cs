using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefsHelper
{
    private const string PreferredLanguageKey = "settings.preferredlanguage";
    private const string VolumeKeyPrefix = "settings.volume.";
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

    private static StoredPrefs loadPrefs()
    {
        StoredPrefs newPrefs = new StoredPrefs();
        newPrefs.preferredLanguage = PlayerPrefs.GetString(PreferredLanguageKey, LocalizationManager.DefaultLanguage);
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
    /// Returns volume of given type, scaled by Master if applicable. Use for most cases.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static float getVolume(VolumeType type)
    {
        if (type == VolumeType.Master)
            return storedPrefs.volumes[0];
        else
            return storedPrefs.volumes[(int)type] * storedPrefs.volumes[0];
    }

    /// <summary>
    /// Returns raw value of volume for given type, unscaled by Master
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static float getVolumeRaw(VolumeType type)
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

}

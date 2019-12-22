using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class DiscordController : MonoBehaviour
{
    // https://github.com/discordapp/discord-rpc/blob/master/examples/button-clicker/Assets/DiscordController.cs

    private const string DefaultStatusString = "default";

    public bool disableInEditor;

    public string applicationId;
    public string optionalSteamId;
    public string details;
    public string largeImageKey;
    public int callbackCalls;
    public UnityEngine.Events.UnityEvent onConnect;
    public UnityEngine.Events.UnityEvent onDisconnect;

    DiscordRpc.RichPresence presence;
    DiscordRpc.EventHandlers handlers;
    bool initialized = false;
    bool ready = false;

    public enum TimeStampType
    {
        Keep,
        Clear,
        CurrentTime
    }


    public void ReadyCallback()
    {
        ++callbackCalls;
        Debug.Log("Discord: ready");
        onConnect.Invoke();
        ready = true;
    }

    public void DisconnectedCallback(int errorCode, string message)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: disconnect {0}: {1}", errorCode, message));
        onDisconnect.Invoke();
    }

    public void ErrorCallback(int errorCode, string message)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: error {0}: {1}", errorCode, message));
    }

    void Update()
    {
        try
        {
            DiscordRpc.RunCallbacks();
        }
        catch (DllNotFoundException e)
        {
            Debug.Log("Discord controller not found! Probably just a non-Windows build idk");
            enabled = false;
        }
    }

    void Start()
    {
        if (initialized)
            return;
#if UNITY_EDITOR
        if (disableInEditor)
        {
            gameObject.SetActive(false);
            return;
        }
#endif
        if (Application.platform != RuntimePlatform.WindowsPlayer)
        {
            gameObject.SetActive(false);
            return;
        }

        Debug.Log("Discord: init");
        callbackCalls = 0;

        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback = ReadyCallback;
        handlers.disconnectedCallback += DisconnectedCallback;
        handlers.errorCallback += ErrorCallback;

        try
        {
            DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
            initialized = true;
        }
        catch (DllNotFoundException e)
        {
            Debug.Log("Discord controller not found! Probably just a non-Windows build idk");
            enabled = false;
        }
    }

    IEnumerator updatePresenceCoroutine()
    {
        while (!ready)
        {
            yield return new WaitForEndOfFrame();
        }
        DiscordRpc.UpdatePresence(ref presence);
    }

    public void updatePresence(string details = DefaultStatusString, string state = DefaultStatusString, TimeStampType startTimeStamp = TimeStampType.Keep)
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (details.Equals(DefaultStatusString))
            details = presence.details;
        if (state.Equals(DefaultStatusString))
            state = presence.state;

        StopCoroutine(updatePresenceCoroutine());
        presence.details = details;
        presence.state = state;
        presence.largeImageKey = largeImageKey;
        switch(startTimeStamp)
        {
            case (TimeStampType.Keep):
                break;
            case (TimeStampType.CurrentTime):
                presence.startTimestamp = getTimeSinceEpoch();
                break;
            case (TimeStampType.Clear):
                presence.startTimestamp = 0;
                break;
        }
        StartCoroutine(updatePresenceCoroutine());
    }

    private int getTimeSinceEpoch()
    {
        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        return (int)t.TotalSeconds;
    }
}

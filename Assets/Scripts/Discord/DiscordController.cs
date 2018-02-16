using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class DiscordController : MonoBehaviour
{
    // https://github.com/discordapp/discord-rpc/blob/master/examples/button-clicker/Assets/DiscordController.cs

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
    bool ready = false;

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
        DiscordRpc.RunCallbacks();
    }

    void Start()
    {

#if UNITY_EDITOR
        if (disableInEditor)
        {
            gameObject.SetActive(false);
            return;
        }
#endif

        Debug.Log("Discord: init");
        callbackCalls = 0;

        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback = ReadyCallback;
        handlers.disconnectedCallback += DisconnectedCallback;
        handlers.errorCallback += ErrorCallback;
        DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
    }

    public void updatePresence(string details, string state, bool resetTimestamp)
    {
        if (!gameObject.activeInHierarchy)
            return;

        presence.details = details;
        presence.state = state;
        presence.largeImageKey = largeImageKey;
        if (resetTimestamp)
            presence.startTimestamp = getTimeSinceEpoch();
        DiscordRpc.UpdatePresence(ref presence);
    }

    public void updatePresenceDetails(string details, bool resetTimestamp)
    {
        updatePresence(details, presence.state, resetTimestamp);
    }

    public void updatePresenceState(string state, bool resetTimestamp)
    {
        updatePresence(presence.details, state, resetTimestamp);
    }

    private int getTimeSinceEpoch()
    {
        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        return (int)t.TotalSeconds;
    }

    //void OnDisable()
    //{
    //    Debug.Log("Discord: shutdown");
    //    DiscordRpc.Shutdown();
    //}

    void OnDestroy()
    {

    }
}
